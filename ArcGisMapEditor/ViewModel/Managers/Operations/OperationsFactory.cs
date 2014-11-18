using System;
using ArcGisMapEditor.Model;

namespace ArcGisMapEditor.ViewModel.Managers.Operations
{
    public class OperationsFactory
    {
        public static Operation Create(
            OperationType operationType, 
            MainViewModel mainViewModel, 
            NetElementType netElementType = NetElementType.Empty)
        {
            switch (operationType)
            {
                case OperationType.Empty: 
                    return new EmptyOperation();

                case OperationType.Select:
                    return new SelectOperation(mainViewModel);

                case OperationType.SelectRegion:
                    return new SelectRegionOperation(mainViewModel);

                case OperationType.MovePoint:
                    return new MovePointOperation(mainViewModel);

                case OperationType.AddNewPointNetElement:
                    return new AddNewPointNetElementOperation(netElementType, mainViewModel);

                default:
                    throw new ArgumentException("Illegal OperationType");
            }
        }
    }
}