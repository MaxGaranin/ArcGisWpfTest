using System;
using System.Windows.Input;
using ArcGisMapEditor.Model;
using ArcGisMapEditor.ViewModel.Containers;
using ArcGisMapEditor.ViewModel.Helpers;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;

namespace ArcGisMapEditor.ViewModel.Managers.Operations
{
    public class AddNewPointNetElementOperation : Operation
    {
        private readonly NetElementType _netElementType;
        private readonly MainViewModel _mainViewModel;
        private readonly GraphicsLayer _operationsLayer;
        private NetElementContainer _operationContainer;
        private bool _isTriggerOn;
        private bool _isMultiAdd;
        
        public AddNewPointNetElementOperation(NetElementType netElementType, MainViewModel mainViewModel)
        {
            OperationType = OperationType.AddNewPointNetElement;

            _netElementType = netElementType;
            _mainViewModel = mainViewModel;
            _operationsLayer = (GraphicsLayer)_mainViewModel.Map.Layers[Constants.OperationLayerID];
            CreateOperationContainer();
            _isTriggerOn = false;
            _isMultiAdd = false;

            SubscribeToMapEvents();
        }

        private void CreateOperationContainer()
        {
            _operationContainer = MapHelper.CreateNetElementContainer(_netElementType, new MapPoint(0, 0));
            _operationContainer.IsVisible = false;
            _operationsLayer.Graphics.Add(_operationContainer.Graphic);
        }

        public override void Finish()
        {
            _operationsLayer.Graphics.Clear();
            UnsubscribeToMapEvents();
            base.Finish();
        }

        #region Mouse and Keyboard EventHandlers

        private void SubscribeToMapEvents()
        {
            _mainViewModel.MapMouseDown += MainViewModelMapMouseDown;
            _mainViewModel.MapMouseMove += MainViewModelMapMouseMove;
            _mainViewModel.MapKeyDown += MainViewModelMapKeyDown;
            _mainViewModel.MapKeyUp += MainViewModelMapKeyUp;
        }

        private void UnsubscribeToMapEvents()
        {
            _mainViewModel.MapMouseDown -= MainViewModelMapMouseDown;
            _mainViewModel.MapMouseMove -= MainViewModelMapMouseMove;
            _mainViewModel.MapKeyDown -= MainViewModelMapKeyDown;
            _mainViewModel.MapKeyUp -= MainViewModelMapKeyUp;
        }

        private void MainViewModelMapMouseDown(object sender, MapPointEventArgs e)
        {
            _operationsLayer.Graphics.Remove(_operationContainer.Graphic);
            _mainViewModel.NetworkManager.AddContainer(_operationContainer);

            if (_isMultiAdd)
            {
                CreateOperationContainer();
                _isTriggerOn = false;
            }
            else
            {
                _mainViewModel.OperationsManager.FinishCurrentOperation();
            }
        }

        private void MainViewModelMapMouseMove(object sender, MapPointEventArgs e)
        {
            if (!_isTriggerOn)
            {
                _isTriggerOn = true;
                _operationContainer.IsVisible = true;
            }
            _operationContainer.MapPoint = e.MapPoint;

            // Debug.Print("{0} : {1}", e.MapPoint.X, e.MapPoint.Y);
        }

        private void MainViewModelMapKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                _isMultiAdd = true;
            }
        }

        private void MainViewModelMapKeyUp(object sender, KeyEventArgs e)
        {
            _isMultiAdd = false;
        }

        #endregion
    }
}