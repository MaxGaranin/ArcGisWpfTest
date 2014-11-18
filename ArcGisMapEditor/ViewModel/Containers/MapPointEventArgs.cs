using System;
using System.Windows;
using Esri.ArcGISRuntime.Geometry;

namespace ArcGisMapEditor.ViewModel.Containers
{
    public class MapPointEventArgs : EventArgs
    {
        public MapPointEventArgs(Point screenPoint, MapPoint mapPoint)
        {
            ScreenPoint = screenPoint;
            MapPoint = mapPoint;
        }

        public Point ScreenPoint { get; set; }
        public MapPoint MapPoint { get; set; }
    }
}