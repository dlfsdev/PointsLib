using PointsLibUi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PointsLibUi
{
    /// <summary>
    /// <see cref="System.Windows.Forms.PropertyGrid"/> does not provide a node expanded/collapsed event.
    /// This class monitors a PropertyGrid and does its best to simulate this event.
    /// Not optimized for grids where the category set changes frequently.
    /// </summary>
    internal sealed class PropertyGridExpandCollapseMonitor : IDisposable
    {
        private readonly PropertyGrid _grid;

        // Unfortunately we can't keep references to the actual GridItems themselves since they are transient
        private readonly IDictionary<string, bool> _previousStates;

        private readonly Timer _timer;

        public event EventHandler<PropertyGridExpandCollapseEventArgs> ExpandStateChanged;

        public PropertyGridExpandCollapseMonitor(PropertyGrid grid)
        {
            _grid = grid;
            _previousStates = new Dictionary<string, bool>();

            _timer = new Timer();
            _timer.Interval = 200;
            _timer.Tick += (o, e) => Refresh();
            _timer.Enabled = true;

            _grid.SelectedObjectsChanged += (s, e) => _previousStates.Clear();
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        private void Refresh()
        {
            GridItem root = _grid.TryGetRoot();
            if (root == null)
                return;

            foreach (var item in root.GridItems.Cast<GridItem>())
            {
                if (!_previousStates.ContainsKey(item.Label))
                    _previousStates.Add(item.Label, !item.Expanded);

                bool previous = _previousStates[item.Label];
                bool current = item.Expanded;

                if (current == previous)
                    continue;

                OnExpandStateChanged(item, current, previous);

                _previousStates[item.Label] = current;
            }
        }

        private void OnExpandStateChanged(GridItem item, bool current, bool previous)
        {
            ExpandStateChanged.RaiseSafe(this,
                new PropertyGridExpandCollapseEventArgs(item, current, previous));
        }
    }
}



internal sealed class PropertyGridExpandCollapseEventArgs : EventArgs
{
    public PropertyGridExpandCollapseEventArgs(GridItem item, bool newExpanded, bool oldExpanded)
    {
        _item = item;
        _newExpanded = newExpanded;
        _oldExpanded = oldExpanded;
    }

    private readonly GridItem _item;
    public GridItem Item { get { return _item; } }

    private readonly bool _newExpanded;
    public bool NewExpanded { get { return _newExpanded; } }

    private readonly bool _oldExpanded;
    public bool OldExpanded { get { return _oldExpanded; } }
}