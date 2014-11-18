using System.Collections.Generic;
using ArcGisMapEditor.Model;
using ArcGisMapEditor.Model.Helpers;
using ArcGisMapEditor.ViewModel.Containers;
using Esri.ArcGISRuntime.Geometry;

namespace ArcGisMapEditor.ViewModel.Helpers
{
    public class MapHelper
    {
        public static IList<NetElementContainer> FindContainers(
            MainViewModel mainViewModel, MapPoint mapPoint, int maxPointsCount = 50)
        {
            var resultContainers = new List<NetElementContainer>();

            var extent = mainViewModel.CurrentExtent;
            var deltaX = (extent.XMax - extent.XMin) / 100;
            var deltaY = (extent.YMax - extent.YMin) / 100;

            var k = 0;
            foreach (var container in mainViewModel.NetworkManager.NetElementContainers)
            {
                var p = (MapPoint)container.Graphic.Geometry;
                if ((p.X < mapPoint.X + deltaX) && (p.X > mapPoint.X - deltaX) &&
                    (p.Y < mapPoint.Y + deltaY) && (p.Y > mapPoint.Y - deltaY))
                {
                    resultContainers.Add(container);

                    k++;
                    if (k >= maxPointsCount) break;
                }
            }

            return resultContainers;
        }

        public static NetElementContainer CreateNetElementContainer(
            NetElementType netElementType, MapPoint mapPoint)
        {
            var netElement = NetElementFactory.CreateNetElement(netElementType);
            var container = new NetElementContainer(netElement);
            container.MapPoint = mapPoint;

            return container;
        }
    }
}