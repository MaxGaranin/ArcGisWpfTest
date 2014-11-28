using System.Windows.Media;
using ArcGisMapEditor.Model;
using ArcGisMapEditor.ViewModel.Containers;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;

namespace ArcGisMapEditor.ViewModel.Managers.Operations
{
    public class SelectRegionOperation : Operation
    {
        private readonly MainViewModel _mainViewModel;
        private readonly GraphicsLayer _operationsLayer;
        private readonly Graphic _selectRegion;
        private MapPoint _initialPoint;
        private bool _isSelect;
        
        public SelectRegionOperation(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _operationsLayer = (GraphicsLayer)_mainViewModel.Map.Layers[Constants.OperationLayerID];

            _selectRegion = CreateRegion();
            _selectRegion.IsVisible = false;
            _operationsLayer.Graphics.Add(_selectRegion);
            _isSelect = false;

            SubscribeToMapEvents();
        }

        private Graphic CreateRegion()
        {
            var mapPoint = new MapPoint(0, 0, SpatialReferences.WebMercator);
            return CreateRegion(mapPoint, mapPoint);
        }

        private Graphic CreateRegion(MapPoint mapPoint1, MapPoint mapPoint2)
        {
            var envelop = new Envelope(mapPoint1, mapPoint2);
            var symbol = new SimpleFillSymbol
            {
                Color = Color.FromArgb(50, 255, 0, 0),
                Outline = new SimpleLineSymbol
                {
                    Color = Color.FromRgb(255, 0, 0),
                    Width = 2
                }
            };

            var region = new Graphic(envelop, symbol);
            return region;
        }

        public override void Finish()
        {
            _operationsLayer.Graphics.Clear();
            UnsubscribeToMapEvents();
            base.Finish();
        }

        private void SubscribeToMapEvents()
        {
            _mainViewModel.MapMouseDown += MainViewModelMapMouseDown;
            _mainViewModel.MapMouseMove += MainViewModelMapMouseMove;
        }

        private void UnsubscribeToMapEvents()
        {
            _mainViewModel.MapMouseDown -= MainViewModelMapMouseDown;
            _mainViewModel.MapMouseMove -= MainViewModelMapMouseMove;
        }

        private void MainViewModelMapMouseDown(object sender, MapPointEventArgs e)
        {
            if (!_isSelect)
            {
                _initialPoint = e.MapPoint;
                _selectRegion.Geometry = new Envelope(_initialPoint, _initialPoint);
                _selectRegion.IsVisible = true;
                _isSelect = true;
            }
            else
            {
                SelectContainersInEnvelop(_selectRegion.Geometry as Envelope);
                _selectRegion.IsVisible = false;
                _isSelect = false;
            }
        }

        private void MainViewModelMapMouseMove(object sender, MapPointEventArgs e)
        {
            if (_isSelect)
            {
                _selectRegion.Geometry = new Envelope(_initialPoint, e.MapPoint);
            }
        }

        private void SelectContainersInEnvelop(Envelope envelop)
        {
            _mainViewModel.UnselectContainers();

            var containers = _mainViewModel.NetworkManager.NetElementContainers;
            foreach (var container in containers)
            {
                var p = container.MapPoint;
                if (p.X >= envelop.XMin && p.X <= envelop.XMax &&
                    p.Y >= envelop.YMin && p.Y <= envelop.YMax)
                {
                    container.IsSelected = true;
                }
            }
        }
    }
}