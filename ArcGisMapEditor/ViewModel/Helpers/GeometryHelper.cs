using ArcGisMapEditor.Model;
using Esri.ArcGISRuntime.Geometry;

namespace ArcGisMapEditor.ViewModel.Helpers
{
    public class GeometryHelper
    {
        public static MapPoint CreateNetElementMapPoint(NetElement netElement)
        {
            var geometry = new MapPoint(netElement.X, netElement.Y);
            return geometry;
        }
    }
}