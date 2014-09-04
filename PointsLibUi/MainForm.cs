using PointsLibUi.Controls;
using PointsLibUi.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PointsLibUi
{
    internal sealed partial class MainForm : Form
    {
        private readonly MainModel _model;

        private readonly PropertyGridExpandCollapseMonitor _optionsTableExpandCollapseMonitor;

        private readonly IDictionary<AlgorithmModel, ProgressPanel> _progressPanels =
            new Dictionary<AlgorithmModel, ProgressPanel>();

        public MainForm( MainModel model, AlgorithmOptionsPropertyGridAdapter options)
        {
            _model = model;

            InitializeComponent();

            CreateProgressPanels();

            _optionsCategoryToModel = new Dictionary<String, AlgorithmModel>(StringComparer.Ordinal)
            {
                { AlgorithmOptionsPropertyGridAdapter.ClosestPairCategory, _model.ClosestPair },
                { AlgorithmOptionsPropertyGridAdapter.ConvexHullCategory, _model.ConvexHull },
                { AlgorithmOptionsPropertyGridAdapter.KMeansCategory, _model.KMeans },
                { AlgorithmOptionsPropertyGridAdapter.TspCategory, _model.Tsp }
            };

            _optionsTable.SelectedObject = options;
            _optionsTable.ExpandAllGridItems();

            _optionsTableExpandCollapseMonitor = new PropertyGridExpandCollapseMonitor(_optionsTable);
            _optionsTableExpandCollapseMonitor.ExpandStateChanged += OnOptionCategoryExpandStateChanged;

            _canvas.DragButton = DragCanvasButton;

            mouseLocationLabel.Text = "";
            boundsLabel.Text = "";

            RegisterEventHandlers();

            OnCanvasBoundsChanged();
            SyncToModel();
        }

        private void CreateProgressPanels()
        {
            AddProgressPanel("Pair", _model.ClosestPair);
            AddProgressPanel("Hull", _model.ConvexHull);
            AddProgressPanel("k-means", _model.KMeans);
            AddProgressPanel("TSP", _model.Tsp);
        }

        private void AddProgressPanel(string name, AlgorithmModel model)
        {
            var panel = new ProgressPanel();
            panel.AlgorithmName = name;
            panel.IndefiniteCompletion = !model.ReportsDefiniteProgress;
            panel.Visible = false;
            panel.CancelClick += (s, e) => model.Cancel();
            panel.Location = new Point(_optionsTable.Left, _optionsTable.Bottom + 6);
            panel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.Controls.Add(panel);
            _progressPanels.Add(model, panel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();

                if (_pointsBitmap != null)
                    _pointsBitmap.Dispose();

                _optionsTableExpandCollapseMonitor.Dispose();

                // don't dispose _model; we don't own it
            }

            base.Dispose(disposing);
        }



        #region Event Handlers

        private const MouseButtons DragCanvasButton = MouseButtons.Middle;
        private const MouseButtons AddPointButton = MouseButtons.Left;
        private const MouseButtons ErasePointButton = MouseButtons.Right;

        private readonly IDictionary<String, AlgorithmModel> _optionsCategoryToModel;

        private void RegisterEventHandlers()
        {
            this.Resize += (s, e) => RedrawScene(true);

            // allow the mouse wheel to zoom the canvas even when other controls have the focus
            this.MouseWheel += _canvas.OnMouseWheel;
            _optionsTable.MouseWheel += _canvas.OnMouseWheel;

            this.FormClosing += (s, e) =>
            {
                _model.ProgressMade -= ProgressMade;
                _model.SettingsChanged -= ModelSettingsChanged;
                _model.CancelAllBlocking();
            };

            _canvas.BoundsChanged += (s, e) => OnCanvasBoundsChanged();

            _model.ProgressMade += ProgressMade;
            _model.SettingsChanged += ModelSettingsChanged;
        }

        void ModelSettingsChanged(object sender, EventArgs e)
        {
            this.DoOnUiThreadNonblocking(() => SyncToModel());
        }

        void ProgressMade(object sender, EventArgs e)
        {
            this.DoOnUiThreadNonblocking(() => { RedrawScene(false); SyncProgressPanelsAsync(); });
        }

        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == AddPointButton)
                _model.Points.Add(_canvas.ToVirtualPoint(e.Location));
            else if (e.Button == ErasePointButton)
                _model.Points.Remove(_canvas.ToVirtualPoint(e.Location), _canvas.ToVirtualDistance(_model.Points.Radius));
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            PointF p = _canvas.ToVirtualPoint(e.Location);
            mouseLocationLabel.Text = String.Format("({0}, {1})", p.X.ToString(), p.Y.ToString());
        }

        private void Canvas_MouseLeave(object sender, EventArgs e)
        {
            mouseLocationLabel.Text = "";
        }

        private void OnCanvasBoundsChanged()
        {
            PointF ul = _canvas.ToVirtualPoint(
                new Point(_canvas.ClientRectangle.Left, _canvas.ClientRectangle.Top));
            PointF lr = _canvas.ToVirtualPoint(
                new Point(_canvas.ClientRectangle.Right - 1, _canvas.ClientRectangle.Bottom - 1));

            boundsLabel.Text = string.Format("{0:G5} x {1:G5}: ({2:G5}, {3:G5}) - ({4:G5}, {5:G5})",
                lr.X - ul.X + 1, lr.Y - ul.Y + 1, ul.X, ul.Y, lr.X, lr.Y);

            RedrawScene(true);
        }

        private void loadPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                using (var reader = new StreamReader(dlg.FileName))
                {
                    var points = PointsSerializer.SerializeIn(reader);
                    _model.CancelAllBlocking();
                    _model.Points.SetPoints(points);
                }
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Failed to load points from file", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void savePointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                using (var writer = new StreamWriter(dlg.FileName))
                {
                    PointsSerializer.SerializeOut(_model.Points.Points, writer);
                }
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Failed to save points to file", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clearPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _model.Points.SetPoints(Enumerable.Empty<PointF>());
        }

        private void algorithmToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            findClosestPairToolStripMenuItem.Enabled =
                _model.ClosestPair.CanTrySolve(_model.Points.Points) && !_model.ClosestPair.IsInProgress;

            solveConvexHullToolStripMenuItem.Enabled =
                _model.ConvexHull.CanTrySolve(_model.Points.Points) && !_model.ConvexHull.IsInProgress;

            solveTravellingSalesmanToolStripMenuItem.Enabled =
                _model.Tsp.CanTrySolve(_model.Points.Points) && !_model.Tsp.IsInProgress;

            solveKMeansToolStripMenuItem.Enabled =
                _model.KMeans.CanTrySolve(_model.Points.Points) && !_model.KMeans.IsInProgress;
        }

        private async void findClosestPairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _optionsTable.SetExpanded(AlgorithmOptionsPropertyGridAdapter.ClosestPairCategory, true);
            await _model.ClosestPair.SolveAsync(_model.Points.Points);
        }

        private async void solveConvexHullToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _optionsTable.SetExpanded(AlgorithmOptionsPropertyGridAdapter.ConvexHullCategory, true);
            await _model.ConvexHull.SolveAsync(_model.Points.Points);
        }

        private async void solveTravellingSalesmanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _optionsTable.SetExpanded(AlgorithmOptionsPropertyGridAdapter.TspCategory, true);
            await _model.Tsp.SolveAsync(_model.Points.Points);
        }

        private async void solveKMeansToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _optionsTable.SetExpanded(AlgorithmOptionsPropertyGridAdapter.KMeansCategory, true);
            await _model.KMeans.SolveAsync(_model.Points.Points);
        }

        private void placeManuallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ManualPointPlacementForm();
            form.Points = _model.Points.Points;

            if (form.ShowDialog() != DialogResult.OK)
                return;

            _model.Points.SetPoints(form.Points);
        }

        private void OnOptionCategoryExpandStateChanged(object sender, PropertyGridExpandCollapseEventArgs e)
        {
            if (e.Item.Label.EqualsOrdinal(AlgorithmOptionsPropertyGridAdapter.PointsCategory))
            {
                if (_model.Points.Visible != e.NewExpanded)
                    _model.Points.Visible = e.NewExpanded;
            }
            else
            {
                AlgorithmModel algModel;
                if (!_optionsCategoryToModel.TryGetValue(e.Item.Label, out algModel))
                    return;
                if (algModel.Visible != e.NewExpanded)
                    algModel.Visible = e.NewExpanded;
            }
        }

        #endregion



        #region Sync and Position

        private void SyncToModel()
        {
            RedrawScene(true);
        }

        private async void SyncProgressPanelsAsync()
        {
            foreach (KeyValuePair<AlgorithmModel, ProgressPanel> pair in _progressPanels)
            {
                var model = pair.Key;
                var panel = pair.Value;

                panel.FractionComplete = model.GetFractionComplete() ?? 0.0;

                TimeSpan? timeRemaining = model.GetApproxTimeRemaining();
                panel.TimeRemainingText = timeRemaining.HasValue ? timeRemaining.Value.ToPrettyString() : "?";
            }

            await PositionProgressPanelsAsync();
        }

        bool _positioning = false;
        private async Task PositionProgressPanelsAsync()
        {
            if (_positioning)
                return;
            _positioning = true;

            var nextLocation = new Point(_optionsTable.Left, _optionsTable.Bottom + 6);

            var newPositions = new List<Tuple<ProgressPanel, Point>>();

            foreach (var pair in _progressPanels)
            {
                var Algorithm = pair.Key;
                var panel = pair.Value;

                if (!Algorithm.IsInProgress)
                {
                    panel.Visible = false;
                    continue;
                }

                if (panel.Location != nextLocation)
                    newPositions.Add(Tuple.Create(panel, nextLocation));

                nextLocation.Y += panel.Height;
            }

            var tasks = new List<Task>();

            // slide down from the bottom up
            newPositions.Sort((l, r) => r.Item1.Location.Y.CompareTo(l.Item1.Location.Y));
            foreach (var pair in newPositions)
                if (pair.Item1.Location.Y < pair.Item2.Y)
                    tasks.Add(SlideToAsync(pair.Item1, pair.Item2, 100));

            // slide up from the top down
            newPositions.Reverse();
            foreach (var pair in newPositions)
                if (pair.Item1.Location.Y > pair.Item2.Y)
                    tasks.Add(SlideToAsync(pair.Item1, pair.Item2, 100));

            await Task.WhenAll(tasks);

            foreach (var pair in _progressPanels)
                if (pair.Key.IsInProgress)
                    pair.Value.Visible = true;

            _positioning = false;
        }

        async Task SlideToAsync(Control c, Point destination, int timeMs)
        {
            Point start = c.Location;
            Point current = start;

            int dx = destination.X - start.X;
            int dy = destination.Y - start.Y;

            double fractionComplete = 0.0;

            var watch = new Stopwatch();
            watch.Start();

            while (fractionComplete < 1.0)
            {
                await Task.Delay(20);

                fractionComplete = Math.Min(watch.ElapsedMilliseconds / (double)timeMs, 1.0);

                current.X = start.X + (int)Math.Round(fractionComplete * dx);
                current.Y = start.Y + (int)Math.Round(fractionComplete * dy);

                if (c.Location != current)
                    c.Location = current;
            }
        }

        #endregion



        #region Drawing

        private static readonly Rectangle _clipBounds = new Rectangle(-100000, -100000, 200000, 200000);

        private Bitmap _pointsBitmap;

        private void RedrawScene(bool mightPointsHaveChanged)
        {
            _canvas.SuspendDrawing();
            _canvas.BackColor = _model.BackgroundColor;

            try
            {
                RedrawSceneImpl(mightPointsHaveChanged);
            }
            finally
            {
                _canvas.ResumeDrawing();

                // Controls outside the canvas fail to redraw themselves properly during drag if we don't do this
                this.Refresh();
            }
        }

        private void RedrawSceneImpl(bool mightPointsHaveChanged)
        {
            if (_canvas.Width <= 0 || _canvas.Height <= 0)
                return;

            Bitmap b;
            using (Graphics g = _canvas.CreateGraphics())
            {
                b = new Bitmap(_canvas.Width, _canvas.Height, g);

                if (_model.Points.Visible && (mightPointsHaveChanged || _pointsBitmap == null))
                {
                    if (_pointsBitmap != null)
                        _pointsBitmap.Dispose();
                    _pointsBitmap = new Bitmap(_canvas.Width, _canvas.Height, g);
                    using (var g2 = Graphics.FromImage(_pointsBitmap))
                        DrawModelPoints(g2);
                }
            }

            using (var g = Graphics.FromImage(b))
            {
                if (_model.Points.Visible)
                    g.DrawImage(_pointsBitmap, new Point(0, 0));

                if (_model.ConvexHull.Visible)
                    DrawConvexHull(g);

                if (_model.Tsp.Visible)
                    DrawTsp(g);

                if (_model.KMeans.Visible)
                    DrawKMeans(g);

                if (_model.ClosestPair.Visible)
                    DrawClosestPair(g);
            }

            var oldImage = _canvas.Image;
            _canvas.Image = b;
            if (oldImage != null)
                oldImage.Dispose();
        }

        private void DrawConvexHull(Graphics g)
        {
            var convexHull = SafeToCanvasClientPoints(_model.ConvexHull.Result);
            var provisionalConvexHull = SafeToCanvasClientPoints(_model.ConvexHull.ProvisionalResult);

            if (!_model.ConvexHull.IsInProgress && convexHull != null)
                DrawPath(g, _model.ConvexHull.ResultColor, convexHull, true);
            else if (provisionalConvexHull != null)
                DrawDashedPath(g, _model.ConvexHull.ResultColor, provisionalConvexHull, false);
        }

        private void DrawTsp(Graphics g)
        {
            var tsp = SafeToCanvasClientPoints(_model.Tsp.Result);
            var provisionalTsp = SafeToCanvasClientPoints(_model.Tsp.ProvisionalResult);

            if (!_model.Tsp.IsInProgress && tsp != null)
                DrawPath(g, _model.Tsp.ResultColor, tsp, true);
            else if (provisionalTsp != null)
                DrawDashedPath(g, _model.Tsp.ResultColor, provisionalTsp, true);
        }

        private void DrawKMeans(Graphics g)
        {
            IEnumerable<PointF> toDraw = null;
            IEnumerable<PointF> result = _model.KMeans.Result;
            IEnumerable<PointF> provisional = _model.KMeans.ProvisionalResult;

            if (!_model.KMeans.IsInProgress && result != null)
                toDraw = result;
            else if (_model.KMeans.ProvisionalResult != null)
                toDraw = provisional;
            else
                return;

            foreach (PointF p in _model.Points.Points)
            {
                PointF closestMean = FindClosest(p, toDraw);
                var line = new Point[] { _canvas.ToClientPoint(p), _canvas.ToClientPoint(closestMean) };
                if (toDraw == result)
                    DrawPath(g, _model.KMeans.ResultColor, line, false);
                else
                    DrawDashedPath(g, _model.KMeans.ResultColor, line, false);
            }

            DrawHashMarks(g, _model.Points.Radius + 2, _model.Points.Color, SafeToCanvasClientPoints(toDraw));
        }

        private PointF FindClosest(PointF p, IEnumerable<PointF> points)
        {
            float? minDistance = null;
            PointF? closestPoint = null;

            foreach (PointF p2 in points)
            {
                float distance = p.SquaredDistanceTo(p2);
                if (!minDistance.HasValue || distance < minDistance.Value)
                {
                    minDistance = distance;
                    closestPoint = p2;
                }
            }

            return closestPoint.Value;
        }

        private void DrawClosestPair(Graphics g)
        {
            var closestPair = SafeToCanvasClientPoints(_model.ClosestPair.Result);
            var provisionalClosestPair = SafeToCanvasClientPoints(_model.ClosestPair.ProvisionalResult);

            if (!_model.ClosestPair.IsInProgress && closestPair != null)
            {
                DrawPath(g, _model.ClosestPair.ResultColor, closestPair, false);
                DrawFilledPoints(g, _model.Points.Radius, _model.ClosestPair.ResultColor, closestPair);
            }
            else if (provisionalClosestPair != null)
            {
                DrawDashedPath(g, _model.ClosestPair.ResultColor, provisionalClosestPair, false);
                DrawFilledPoints(g, _model.Points.Radius, _model.ClosestPair.ResultColor, provisionalClosestPair);
            }
        }

        private void DrawModelPoints(Graphics g)
        {
            var points = SafeToCanvasClientPoints(_model.Points.Points);
            DrawPoints(g, _model.Points.Radius, _model.Points.Color, points);
        }

        private void DrawPoints(Graphics g, int radius, Color color, IEnumerable<Point> points)
        {
            DrawPointsImpl(g, radius, color, false, points);
        }

        private void DrawFilledPoints(Graphics g, int radius, Color color, IEnumerable<Point> points)
        {
            DrawPointsImpl(g, radius, color, true, points);
        }

        private void DrawPointsImpl(Graphics g, int radius, Color color, bool filled, IEnumerable<Point> points)
        {
            using (var pen = new Pen(color, 1))
            using (var brush = filled ? new SolidBrush(color) : null)
            {
                foreach (Point point in points)
                {
                    var r = new Rectangle(point.X - radius, point.Y - radius, radius * 2 + 1, radius * 2 + 1);

                    // for efficiency and overflow protection
                    if (!_clipBounds.IntersectsWith(r))
                        continue;

                    g.DrawEllipse(pen, r);

                    if (!filled)
                        continue;

                    g.FillEllipse(brush, r);
                }
            }
        }

        private void DrawPath(Graphics g, Color color, IEnumerable<Point> points, bool isClosed)
        {
            using (var pen = new Pen(color))
                DrawPathImpl(g, pen, points, isClosed);
        }

        private void DrawDashedPath(Graphics g, Color color, IEnumerable<Point> points, bool isClosed)
        {
            using (var pen = new Pen(color))
            {
                pen.DashPattern = new float[] { 3.0f, 3.0f };
                DrawPathImpl(g, pen, points, isClosed);
            }
        }

        private void DrawPathImpl(Graphics g, Pen pen, IEnumerable<Point> points, bool isClosed)
        {
            var pointsArray = points.ToArray();

            if (pointsArray.Length == 0)
                return;

            if (pointsArray.Length == 1)
                g.DrawRectangle(pen, pointsArray[0].X, pointsArray[0].Y, 1, 1);
            else
            {
                if (isClosed)
                    g.DrawPolygon(pen, pointsArray);
                else
                    g.DrawLines(pen, pointsArray);
            }
        }

        private void DrawHashMarks(Graphics g, int radius, Color color, IEnumerable<Point> points)
        {
            var pointsArray = points.ToArray();

            using (var pen = new Pen(color))
            {
                foreach (Point point in points)
                {
                    g.DrawLine(pen, new Point(point.X - radius, point.Y), new Point(point.X + radius, point.Y));
                    g.DrawLine(pen, new Point(point.X, point.Y - radius), new Point(point.X, point.Y + radius));
                }
            }
        }

        #endregion



        #region Misc

        private IEnumerable<Point> SafeToCanvasClientPoints(IEnumerable<PointF> points)
        {
            if (points == null)
                return null;
            return points.Select(p => _canvas.ToClientPoint(p));
        }

        #endregion
    }
}