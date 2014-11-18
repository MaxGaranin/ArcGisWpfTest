using ArcGisMapEditor.Model;
using ArcGisMapEditor.ViewModel.Containers;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;

namespace ArcGisMapEditor.ViewModel.Managers.Operations
{
    public class SelectRegionOperation : Operation
    {
        private readonly MainViewModel _mainViewModel;
        private readonly GraphicsLayer _operationsLayer;
        private Graphic _selectedRegion;
        private MapPoint _initialPoint;
        private bool _isSelect;
        
        public SelectRegionOperation(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _operationsLayer = (GraphicsLayer)_mainViewModel.Map.Layers[Constants.OperationLayerID];

            SubscribeToMapEvents();
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
            _mainViewModel.MapMouseUp += MainViewModelMapMouseUp;
            _mainViewModel.MapMouseMove += MainViewModelMapMouseMove;
        }

        private void UnsubscribeToMapEvents()
        {
            _mainViewModel.MapMouseDown -= MainViewModelMapMouseDown;
            _mainViewModel.MapMouseUp -= MainViewModelMapMouseUp;
            _mainViewModel.MapMouseMove -= MainViewModelMapMouseMove;
        }

        private void MainViewModelMapMouseDown(object sender, MapPointEventArgs e)
        {
            _isSelect = true;
            _initialPoint = e.MapPoint;
            _selectedRegion = new Graphic(new Envelope(_initialPoint, _initialPoint));
        }

        private void MainViewModelMapMouseUp(object sender, MapPointEventArgs e)
        {
            if (_isSelect)
            {

                _isSelect = false;
            }
        }

        private void MainViewModelMapMouseMove(object sender, MapPointEventArgs e)
        {
            if (_isSelect)
            {
                _selectedRegion.Geometry = new Envelope(_initialPoint, e.MapPoint);

            }
        }

    }
}