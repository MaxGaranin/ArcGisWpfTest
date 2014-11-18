using System;
using System.Diagnostics;
using System.Windows.Input;
using ArcGisMapEditor.ViewModel.Containers;
using ArcGisMapEditor.ViewModel.Helpers;
using Esri.ArcGISRuntime.Geometry;

namespace ArcGisMapEditor.ViewModel.Managers.Operations
{
    public class SelectOperation : Operation
    {
        private readonly MainViewModel _mainViewModel;
        private bool _isMultiSelect;

        public SelectOperation(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _isMultiSelect = false;

            SubscribeToMapEvents();
        }

        public override void Finish()
        {
            _mainViewModel.UnselectContainers();
            UnsubscribeToMapEvents();
            base.Finish();
        }

        private void SubscribeToMapEvents()
        {
            _mainViewModel.MapMouseDown += MainViewModelMapMouseDown;
            _mainViewModel.MapKeyDown += MainViewModelMapKeyDown;
            _mainViewModel.MapKeyUp += MainViewModelMapKeyUp;
        }

        private void UnsubscribeToMapEvents()
        {
            _mainViewModel.MapMouseDown -= MainViewModelMapMouseDown;
            _mainViewModel.MapKeyDown -= MainViewModelMapKeyDown;
            _mainViewModel.MapKeyUp -= MainViewModelMapKeyUp;
        }

        private void MainViewModelMapMouseDown(object sender, MapPointEventArgs e)
        {
            if (!_isMultiSelect) _mainViewModel.UnselectContainers();
            
            SelectContainer(e.MapPoint);            
        }

        private void MainViewModelMapKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                _isMultiSelect = true;
            }
            else if (e.Key == Key.Delete)
            {
                _mainViewModel.NetworkManager.RemoveSelectedElements();
            }
        }

        private void MainViewModelMapKeyUp(object sender, KeyEventArgs e)
        {
            _isMultiSelect = false;
        }

        private void SelectContainer(MapPoint mapPoint)
        {
            var containers = MapHelper.FindContainers(_mainViewModel, mapPoint);
            if (containers.Count == 0) return;

            foreach (var container in containers)
            {
                container.IsSelected = !container.IsSelected;    
            }
        }
    }
}