using PointsLibInterop;
using PointsLibUi.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PointsLibUi
{
    internal sealed class MainModel : IDisposable
    {
        private readonly IEnumerable<AlgorithmModel> _algorithmModels;

        public event EventHandler ProgressMade;
        public event EventHandler SettingsChanged;

        public MainModel()
        {
            Points.PointsChanged += (o, e) => { OnPointsChanged(); OnSettingsChanged(); };
            Points.SettingsChanged += (o, e) => OnSettingsChanged();

            _algorithmModels = new AlgorithmModel[] { ClosestPair, ConvexHull, Tsp, KMeans };

            foreach (var model in _algorithmModels)
            {
                model.ProgressMade += (s, e) => OnProgressMade();
                model.SettingsChanged += (s, e) => OnSettingsChanged();
            }
        }

        private Color _backgroundColor;
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; OnSettingsChanged(); }
        }

        private readonly PointsModel _points = new PointsModel();
        public PointsModel Points
        {
            get { return _points; }
        }

        private readonly ClosestPairModel _closestPairModel = new ClosestPairModel();
        public ClosestPairModel ClosestPair
        {
            get { return _closestPairModel; }
        }

        private readonly ConvexHullModel _convexHullModel = new ConvexHullModel();
        public ConvexHullModel ConvexHull
        {
            get { return _convexHullModel; }
        }

        private readonly TspModel _tspModel = new TspModel();
        public TspModel Tsp
        {
            get { return _tspModel; }
        }

        private readonly KMeansModel _kMeansModel = new KMeansModel();
        public KMeansModel KMeans
        {
            get { return _kMeansModel; }
        }

        public void CancelAll()
        {
            foreach (var model in _algorithmModels)
                model.Cancel();
        }

        public bool IsAnyInProgress()
        {
            foreach (var model in _algorithmModels)
                if (model.IsInProgress)
                    return true;
            return false;
        }

        public void CancelAllBlocking()
        {
            CancelAll();

            while (IsAnyInProgress())
                Thread.Sleep(10);
        }

        public async Task CancelAllAndWaitAsync()
        {
            CancelAll();

            while (IsAnyInProgress())
                await Task.Delay(10).ConfigureAwait(true);
        }

        private void OnProgressMade()
        {
            ProgressMade.RaiseSafe(this, EventArgs.Empty);
        }

        private void OnSettingsChanged()
        {
            SettingsChanged.RaiseSafe(this, EventArgs.Empty);
        }

        private void OnPointsChanged()
        {
            CancelAll();
            ClearResults();
        }

        private void ClearResults()
        {
            foreach (var model in _algorithmModels)
                model.ClearResults();
        }

        public void Dispose()
        {
            _closestPairModel.Dispose();
            _convexHullModel.Dispose();
            _tspModel.Dispose();
            _kMeansModel.Dispose();
        }
    }
}
