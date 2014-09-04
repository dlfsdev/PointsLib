using PointsLibInterop;
using PointsLibUi.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;

namespace PointsLibUi
{
    /// <summary>
    /// Facade/adapter that converts the subset of <see cref="PointsLibUi.MainModel"/> properties that should be
    /// displayed to the user as options into a form that plays well with <see cref="System.Windows.Forms.PropertyGrid"/>
    /// </summary>
    internal sealed partial class AlgorithmOptionsPropertyGridAdapter
    {
        // Leading tabs are a hack to get the sort order we want (they aren't displayed)
        public const string PointsCategory = "\tPoints";
        public const string ClosestPairCategory = "Closest Pair";
        public const string ConvexHullCategory = "Convex Hull";
        public const string TspCategory = "Travelling Salesman";
        public const string KMeansCategory = "k-Means";

        private readonly MainModel _model;

        public AlgorithmOptionsPropertyGridAdapter(MainModel model)
        {
            _model = model;
        }
    }



    // points
    partial class AlgorithmOptionsPropertyGridAdapter
    {
        [Category(PointsCategory)]
        [Description("Point size")]
        [DisplayName("Size")]
        [Editor(typeof(SliderEditor<int, PointSizeSliderEditorProperties>), typeof(UITypeEditor))]
        public int PointsRadius
        {
            get { return _model.Points.Radius; }
            set { _model.Points.Radius = PointSizeSliderEditorProperties.ClampValue(value); }
        }

        private bool ShouldSerializePointsRadius()
        {
            return false;
        }

        [Category(PointsCategory)]
        [Description("Point color")]
        [DisplayName("Color")]
        public Color PointsColor
        {
            get { return _model.Points.Color; }
            set { _model.Points.Color = value; }
        }

        private bool ShouldSerializePointsColor()
        {
            return false;
        }

        [Category(PointsCategory)]
        [Description("Background color")]
        [DisplayName("Background")]
        public Color BackgroundColor
        {
            get { return _model.BackgroundColor; }
            set { _model.BackgroundColor = value; }
        }

        private bool ShouldSerializeBackgroundColor()
        {
            return false;
        }

        private class PointSizeSliderEditorProperties : SliderEditorProperties
        {
            public const int MIN = 0;
            public const int MAX = 5;

            public int MinValue { get { return MIN; } }
            public int MaxValue { get { return MAX; } }

            public static int ClampValue(int value)
            {
                return value.Clamp(MIN, MAX);
            }
        }
    }



    // closest pair
    partial class AlgorithmOptionsPropertyGridAdapter
    {
        private ExponentialSpeedDelayConverter _closestPairConverter =
            new ExponentialSpeedDelayConverter(SpeedSliderEditorProperties.MAX_SPEED, 2000);
        
        [Category(ClosestPairCategory)]
        [Description("How fast should the algorithm run from 0 (slowest) to 10 (fastest)?")]
        [DisplayName("Speed")]
        [Editor(typeof(SliderEditor<int, SpeedSliderEditorProperties>), typeof(UITypeEditor))]
        public int ClosestPairSpeed
        {
            get { return _closestPairConverter.DelayToSpeed(_model.ClosestPair.ThrottleMs); }
            set { _model.ClosestPair.ThrottleMs = _closestPairConverter.SpeedToDelay(value); }
        }

        private bool ShouldSerializeClosestPairSpeed()
        {
            return false;
        }

        [Category(ClosestPairCategory)]
        [Description("Closest pair color")]
        [DisplayName("\tColor")]
        public Color ClosestPairResultColor
        {
            get { return _model.ClosestPair.ResultColor; }
            set { _model.ClosestPair.ResultColor = value; }
        }

        private bool ShouldSerializeClosestPairResultColor()
        {
            return false;
        }
    }


    
    // convex hull
    partial class AlgorithmOptionsPropertyGridAdapter
    {
        private LinearSpeedDelayConverter _convexHullConverter =
            new LinearSpeedDelayConverter(SpeedSliderEditorProperties.MAX_SPEED, 1000);

        [Category(ConvexHullCategory)]
        [Description("How fast should the algorithm run from 0 (slowest) to 10 (fastest)?")]
        [DisplayName("\tSpeed")]
        [Editor(typeof(SliderEditor<int, SpeedSliderEditorProperties>), typeof(UITypeEditor))]
        public int ConvexHullSpeed
        {
            get { return _convexHullConverter.DelayToSpeed(_model.ConvexHull.ThrottleMs); }
            set { _model.ConvexHull.ThrottleMs = _convexHullConverter.SpeedToDelay(value); }
        }

        private bool ShouldSerializeConvexHullSpeed()
        {
            return false;
        }

        [Category(ConvexHullCategory)]
        [Description("Convex hull color")]
        [DisplayName("\t\tColor")]
        public Color ConvexHullResultColor
        {
            get { return _model.ConvexHull.ResultColor; }
            set { _model.ConvexHull.ResultColor = value; }
        }

        private bool ShouldSerializeConvexHullResultColor()
        {
            return false;
        }
    }


    
    // tsp
    partial class AlgorithmOptionsPropertyGridAdapter
    {
        [Category(TspCategory)]
        [Description("Number of worker threads to use. Choosing the maximum value may make your computer unresponsive while the algorithm is running.")]
        [DisplayName("Workers")]
        [Editor(typeof(SliderEditor<int, TspNumThreadsSliderEditorProperties>), typeof(UITypeEditor))]
        public int TspNumThreads
        {
            get { return (int)_model.Tsp.NumThreads; }
            set { _model.Tsp.NumThreads = TspNumThreadsSliderEditorProperties.ClampValue(value); }
        }

        private bool ShouldSerializeTspNumThreads()
        {
            return false;
        }

        [Category(TspCategory)]
        [Description("Travelling salesman route color")]
        [DisplayName("\t\t\tColor")]
        public Color TspResultColor
        {
            get { return _model.Tsp.ResultColor; }
            set { _model.Tsp.ResultColor = value; }
        }

        private bool ShouldSerializeTspResultColor()
        {
            return false;
        }

        private class TspNumThreadsSliderEditorProperties : SliderEditorProperties
        {
            public const int MIN = 1;
            public static readonly int MAX = Environment.ProcessorCount;

            public int MinValue { get { return MIN; } }
            public int MaxValue { get { return MAX; } }

            public static int ClampValue(int value)
            {
                return value.Clamp(MIN, MAX);
            }
        }
    }



    // k-means
    partial class AlgorithmOptionsPropertyGridAdapter
    {
        [Category(KMeansCategory)]
        [Description("Number of clusters to create")]
        [DisplayName("k")]
        public int KMeansK
        {
            get { return _model.KMeans.K; }
            set { _model.KMeans.K = value; }
        }

        private bool ShouldSerializeKMeansK()
        {
            return false;
        }

        [Category(KMeansCategory)]
        [Description("Technique used to select the initial means")]
        [DisplayName("Initialization")]
        public KMeansInitialMeansStrategy KMeansInitialMeansStrategy
        {
            get { return _model.KMeans.InitialMeansStrategy; }
            set { _model.KMeans.InitialMeansStrategy = value; }
        }

        private bool ShouldSerializeKMeansInitialMeansStrategy()
        {
            return false;
        }

        [Category(KMeansCategory)]
        [Description("Number of times to repeat the heuristic algorithm")]
        [DisplayName("Repetitions")]
        public int KMeansRepetitions
        {
            get { return _model.KMeans.Repetitions; }
            set { _model.KMeans.Repetitions = value; }
        }

        private bool ShouldSerializeKMeansRepetitions()
        {
            return false;
        }

        private ExponentialSpeedDelayConverter _kMeansConverter =
            new ExponentialSpeedDelayConverter(SpeedSliderEditorProperties.MAX_SPEED, 2000);

        [Category(KMeansCategory)]
        [Description("How fast should the algorithm run from 0 (slowest) to 10 (fastest)?")]
        [DisplayName("\tSpeed")]
        [Editor(typeof(SliderEditor<int, SpeedSliderEditorProperties>), typeof(UITypeEditor))]
        public int KMeansSpeed
        {
            get { return _kMeansConverter.DelayToSpeed(_model.KMeans.ThrottleMs); }
            set { _model.KMeans.ThrottleMs = _kMeansConverter.SpeedToDelay(value); }
        }

        private bool ShouldSerializeKMeansSpeed()
        {
            return false;
        }

        [Category(KMeansCategory)]
        [Description("k-means color")]
        [DisplayName("\t\t\t\tColor")]
        public Color KMeansColor
        {
            get { return _model.KMeans.ResultColor; }
            set { _model.KMeans.ResultColor = value; }
        }

        private bool ShouldSerializeKMeansColor()
        {
            return false;
        }
    }



    // speed helpers
    internal sealed partial class AlgorithmOptionsPropertyGridAdapter
    {
        private class SpeedSliderEditorProperties : SliderEditorProperties
        {
            public const int MIN_SPEED = 0;
            public const int MAX_SPEED = 10;

            public int MinValue { get { return MIN_SPEED; } }
            public int MaxValue { get { return MAX_SPEED; } }

            public static int ClampValue(int value)
            {
                return value.Clamp(MIN_SPEED, MAX_SPEED);
            }
        }

        private class LinearSpeedDelayConverter
        {
            private readonly int _maxSpeed;
            private readonly int _maxDelay;
            private readonly double _msPerSpeed;

            public LinearSpeedDelayConverter(int maxSpeed, int maxDelay)
            {
                _maxDelay = maxDelay;
                _maxSpeed = maxSpeed;
                _msPerSpeed = (double)_maxDelay / _maxSpeed;
            }

            public int DelayToSpeed(int delay)
            {
                var val = (int)(_maxSpeed - delay / _msPerSpeed);
                return val.Clamp(0, _maxSpeed);
            }

            public int SpeedToDelay(int speed)
            {
                var val = (_maxSpeed - speed).Clamp(0, _maxSpeed) * _msPerSpeed;
                return (int)val;
            }
        }

        private class ExponentialSpeedDelayConverter
        {
            private readonly int _maxSpeed;
            private readonly int _maxDelay;
            private readonly double _root;
            private readonly double _logRoot;

            public ExponentialSpeedDelayConverter(int maxSpeed, int maxDelay)
            {
                _maxSpeed = maxSpeed;
                _maxDelay = maxDelay;
                _root = Math.Pow(_maxDelay, 1.0 / (_maxSpeed - 1));
                _logRoot = Math.Log(_root);
            }

            public int DelayToSpeed(int delay)
            {
                if (delay == 0)
                    return _maxSpeed;
                var val = _maxSpeed - (int)Math.Round((Math.Log(delay) / _logRoot)) - 1;
                return val.Clamp(0, _maxSpeed);
            }

            public int SpeedToDelay(int speed)
            {
                speed = speed.Clamp(0, _maxSpeed);
                if(speed == _maxSpeed)
                    return 0;
                var val = (int)Math.Round(Math.Pow(_root, _maxSpeed - speed - 1));
                return val;
            }
        }
    }
}
