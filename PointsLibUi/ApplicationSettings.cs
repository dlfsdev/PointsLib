using PointsLibInterop;
using System;
using System.Drawing;
using System.Xml.Serialization;

namespace PointsLibUi
{
    public sealed class ApplicationSettings
    {
        public ApplicationSettings()
        {
            // these are effectively the default values for all the settings
            PointsRadius = 3;
            PointsColor = Color.Blue;
            BackgroundColor = Color.White;
            
            ClosestPairSpeed = 10;
            ClosestPairResultColor = Color.Magenta;
            
            ConvexHullSpeed = 10;
            ConvexHullResultColor = Color.DarkGreen;
            
            TspNumThreads = Environment.ProcessorCount - 1;
            TspResultColor = Color.Red;
            
            KMeansK = 5;
            KMeansInitialMeansStrategy = KMeansInitialMeansStrategy.PlusPlus;
            KMeansRepetitions = 5;
            KMeansSpeed = 10;
            KMeansColor = Color.OrangeRed;
        }

        public int PointsRadius { get; set; }

        [XmlElement(typeof(XmlColor))]
        public Color PointsColor { get; set; }

        [XmlElement(typeof(XmlColor))]
        public Color BackgroundColor { get; set; }

        public int ClosestPairSpeed { get; set; }

        [XmlElement(typeof(XmlColor))]
        public Color ClosestPairResultColor { get; set; }

        public int ConvexHullSpeed { get; set; }

        [XmlElement(typeof(XmlColor))]
        public Color ConvexHullResultColor { get; set; }

        public int TspNumThreads { get; set; }

        [XmlElement(typeof(XmlColor))]
        public Color TspResultColor { get; set; }

        public int KMeansK { get; set; }

        public KMeansInitialMeansStrategy KMeansInitialMeansStrategy { get; set; }

        public int KMeansRepetitions { get; set; }

        public int KMeansSpeed { get; set; }

        [XmlElement(typeof(XmlColor))]
        public Color KMeansColor { get; set; }
    }
}