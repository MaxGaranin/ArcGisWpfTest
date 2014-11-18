using System;
using System.Windows.Input;
using ArcGisMapEditor.Model;
using ArcGisMapEditor.ViewModel.Managers.Operations;

namespace ArcGisMapEditor.ViewModel.Managers
{
    public class OperationsManager
    {
        private readonly MainViewModel _mainViewModel;

        public OperationsManager(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            CurrentOperation = CreateEmptyOperation();

            SubscribeToMapEvents();
        }
        
        private void SubscribeToMapEvents()
        {
            _mainViewModel.MapKeyDown += MainViewModelMapKeyDown;
        }

        private void MainViewModelMapKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                FinishCurrentOperation();
            }
        }

        #region Properties

        public Operation CurrentOperation { get; private set; } 
        
        #endregion

        #region Operations

        public void BeginNewOperation(OperationType operationType, NetElementType netElementType = NetElementType.Empty)
        {
            CurrentOperation.Finish();
            CurrentOperation = OperationsFactory.Create(operationType, _mainViewModel, netElementType);
        }

        public void FinishCurrentOperation()
        {
            CurrentOperation.Finish();
            CurrentOperation = CreateEmptyOperation();

            _mainViewModel.TurnOnSelectMode();
        }

        private Operation CreateEmptyOperation()
        {
            return OperationsFactory.Create(OperationType.Empty, _mainViewModel);
        }

        #endregion
    }
}