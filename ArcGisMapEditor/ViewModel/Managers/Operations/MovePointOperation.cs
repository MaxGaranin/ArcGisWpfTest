using ArcGisMapEditor.Model;
using ArcGisMapEditor.ViewModel.Containers;
using ArcGisMapEditor.ViewModel.Helpers;
using Esri.ArcGISRuntime.Layers;

namespace ArcGisMapEditor.ViewModel.Managers.Operations
{
    public class MovePointOperation : Operation
    {
        private readonly MainViewModel _mainViewModel;
        private readonly GraphicsLayer _operationsLayer;
        private NetElementContainer _selectedContainer;
        private NetElementContainer _operationContainer;
        private bool _isSelect;
        private bool _isMove;

        public MovePointOperation(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _operationsLayer = (GraphicsLayer)_mainViewModel.Map.Layers[Constants.OperationLayerID];
            _isSelect = true;
            _isMove = false;

            SubscribeToMapEvents();
        }

        public override void Finish()
        {
            if (_selectedContainer != null) _selectedContainer.IsSelected = false;
            _operationsLayer.Graphics.Clear();
            UnsubscribeToMapEvents();
            base.Finish();
        }

        #region Mouse and Keyboard EventHandlers

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
            if (_isSelect)
            {
                var containers = MapHelper.FindContainers(_mainViewModel, e.MapPoint);
                if (containers.Count == 0) return;

                _selectedContainer = containers[0];
                _mainViewModel.SelectOneContainer(_selectedContainer);
                _operationContainer = MapHelper.CreateNetElementContainer(_selectedContainer.NetElement.ElementType, e.MapPoint);
                _operationsLayer.Graphics.Add(_operationContainer.Graphic);
                _isMove = true;
                _isSelect = false;
            }
            else if (_isMove)
            {
                _selectedContainer.MapPoint = _operationContainer.MapPoint;
                _selectedContainer.IsSelected = false;
                _operationsLayer.Graphics.Clear();
                _isMove = false;
                _isSelect = true;
            }
        }

        private void MainViewModelMapMouseMove(object sender, MapPointEventArgs e)
        {
            if (_isMove)
            {
                _operationContainer.MapPoint = e.MapPoint;
            }
        }

        #endregion

    }
}