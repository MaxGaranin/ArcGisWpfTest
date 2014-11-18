using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ArcGisMapEditor.Model;
using ArcGisMapEditor.Model.Helpers;
using ArcGisMapEditor.ViewModel.Containers;
using ArcGisMapEditor.ViewModel.Managers;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace ArcGisMapEditor.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private BingLayer _geographicLayer;
        private GraphicsLayer _operationLayer;

        #endregion

        #region Constructor

        public MainViewModel()
        {
            Map = CreateMap();
            Editor = new Editor();

            var network = NetworkHelper.GenerateNetwork();
            NetworkManager = new NetworkManager(network, Map);

            OperationsManager = new OperationsManager(this);
            TurnOnSelectMode();
        }

        private Map CreateMap()
        {
            var map = new Map {Layers = CreateLayerCollection()};
            return map;
        }

        private LayerCollection CreateLayerCollection()
        {
            var layerCollection = new LayerCollection();

            _geographicLayer = CreateGeographicLayer();
            layerCollection.Add(_geographicLayer);

            // Layers for elements
            var elementTypes = Enum.GetValues(typeof (NetElementType)).Cast<NetElementType>().ToList();
            foreach (var elementType in elementTypes)
            {
                layerCollection.Add(new GraphicsLayer
                {
                    ID = elementType.ToString(),
                    SelectionColor = Colors.SandyBrown
                });
            }

            _operationLayer = new GraphicsLayer { ID = Constants.OperationLayerID };
            layerCollection.Add(_operationLayer);

            return layerCollection;
        }

        private static BingLayer CreateGeographicLayer()
        {
            return new BingLayer
            {
                ID = Constants.GeographicsLayerID,
                Key = Constants.BingMapKey,
                MapStyle = BingLayer.LayerType.Road,
                Culture = new CultureInfo("ru-ru")
            };
        }

        #endregion

        #region Properties

        public Map Map { get; private set; }
        public Editor Editor { get; private set; }

        public NetworkManager NetworkManager { get; private set; }
        public OperationsManager OperationsManager { get; private set; }

        #region CurrentCoord

        private Coord _currentCoord;

        public Coord CurrentCoord
        {
            get { return _currentCoord; }
            set
            {
                if (Equals(value, _currentCoord)) return;
                _currentCoord = value;
                RaisePropertyChanged("CurrentCoord");
            }
        }

        #endregion

        #region CurrentExtent

        private Envelope _currentExtent;

        public Envelope CurrentExtent
        {
            get { return _currentExtent; }
            set
            {
                if (Equals(value, _currentExtent)) return;
                _currentExtent = value;
                RaisePropertyChanged("CurrentExtent");
            }
        }

        #endregion

        #region IsSelectMode

        private bool _isSelectMode;

        public bool IsSelectMode
        {
            get { return _isSelectMode; }
            set
            {
                _isSelectMode = value;
                RaisePropertyChanged("IsSelectMode");

                if (value)
                {
                    OperationsManager.BeginNewOperation(OperationType.Select);
                }
            }
        }

        #endregion

        #endregion

        #region Commands

        #region AddNewPointNetElement

        private RelayCommand<NetElementType> _addNewPointNetElementCommand;

        public ICommand AddNewPointNetElementCommand
        {
            get
            {
                return _addNewPointNetElementCommand
                       ?? (_addNewPointNetElementCommand = new RelayCommand<NetElementType>(AddNewPointNetElement));
            }
        }

        private void AddNewPointNetElement(NetElementType netElementType)
        {
            OperationsManager.BeginNewOperation(OperationType.AddNewPointNetElement, netElementType);
        }

        #endregion

        #region AddNewPipeNetElement

        private RelayCommand<NetElementType> _addNewPipeNetElementCommand;

        public ICommand AddNewPipeNetElementCommand
        {
            get
            {
                return _addNewPipeNetElementCommand
                       ?? (_addNewPipeNetElementCommand = new RelayCommand<NetElementType>(AddNewPipeNetElement));
            }
        }

        private void AddNewPipeNetElement(NetElementType netElementType)
        {
            OperationsManager.BeginNewOperation(OperationType.AddNewPipeNetElement, netElementType);
        }

        #endregion

        #region SelectRegion

        private RelayCommand _selectRegionCommand;

        public ICommand SelectRegionCommand
        {
            get
            {
                return _selectRegionCommand
                       ?? (_selectRegionCommand = new RelayCommand(SelectRegion));
            }
        }

        private void SelectRegion()
        {
            OperationsManager.BeginNewOperation(OperationType.Select);
        }

        #endregion

        #region MovePoint

        private RelayCommand _movePointCommand;

        public ICommand MovePointCommand
        {
            get
            {
                return _movePointCommand
                       ?? (_movePointCommand = new RelayCommand(MovePoint));
            }
        }

        private void MovePoint()
        {
            OperationsManager.BeginNewOperation(OperationType.MovePoint);
        }

        #endregion

        #region MapEvents

        #region ExtentChanged

        private RelayCommand<MapView> _extentChangedCommand;

        public ICommand ExtentChangedCommand
        {
            get
            {
                return _extentChangedCommand
                       ?? (_extentChangedCommand = new RelayCommand<MapView>(ExtentChanged));
            }
        }

        private void ExtentChanged(MapView mapView)
        {
            CurrentExtent = mapView.Extent;
        }

        #endregion

        #region OnMouseDown

        private RelayCommand<MouseButtonEventArgs> _onMouseDownCommand;

        public ICommand OnMouseDownCommand
        {
            get
            {
                return _onMouseDownCommand
                       ?? (_onMouseDownCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseDown));
            }
        }

        private void OnMouseDown(MouseButtonEventArgs e)
        {
            var args = GetMapPointEventArgs(e);
            MapMouseDown(this, args);
        }

        #endregion

        #region OnMouseUp

        private RelayCommand<MouseButtonEventArgs> _onMouseUpCommand;

        public ICommand OnMouseUpCommand
        {
            get
            {
                return _onMouseUpCommand
                       ?? (_onMouseUpCommand = new RelayCommand<MouseButtonEventArgs>(OnMouseUp));
            }
        }

        private void OnMouseUp(MouseButtonEventArgs e)
        {
            var args = GetMapPointEventArgs(e);
            MapMouseUp(this, args);
        }

        #endregion

        #region OnMouseMove

        private RelayCommand<MouseEventArgs> _onMouseMoveCommand;

        public ICommand OnMouseMoveCommand
        {
            get
            {
                return _onMouseMoveCommand
                       ?? (_onMouseMoveCommand = new RelayCommand<MouseEventArgs>(OnMouseMove));
            }
        }

        private void OnMouseMove(MouseEventArgs e)
        {
            var args = GetMapPointEventArgs(e);
            MapMouseMove(this, args);

            var mapPoint = args.MapPoint;
            CurrentCoord = new Coord(mapPoint.X, mapPoint.Y);
        }

        #endregion

        #region OnKeyDown

        private RelayCommand<KeyEventArgs> _onKeyDownCommand;

        public ICommand OnKeyDownCommand
        {
            get
            {
                return _onKeyDownCommand
                       ?? (_onKeyDownCommand = new RelayCommand<KeyEventArgs>(OnKeyDown));
            }
        }

        private void OnKeyDown(KeyEventArgs e)
        {
            MapKeyDown(this, e);
        }

        #endregion

        #region OnKeyUp

        private RelayCommand<KeyEventArgs> _onKeyUpCommand;

        public ICommand OnKeyUpCommand
        {
            get
            {
                return _onKeyUpCommand
                       ?? (_onKeyUpCommand = new RelayCommand<KeyEventArgs>(OnKeyUp));
            }
        }

        private void OnKeyUp(KeyEventArgs e)
        {
            MapKeyUp(this, e);
        }

        #endregion

        #endregion

        #endregion

        #region Events

        public event EventHandler<MapPointEventArgs> MapMouseDown = (s, e) => { };
        public event EventHandler<MapPointEventArgs> MapMouseUp = (s, e) => { };
        public event EventHandler<MapPointEventArgs> MapMouseMove = (s, e) => { };

        public event EventHandler<KeyEventArgs> MapKeyDown = (s, e) => { };
        public event EventHandler<KeyEventArgs> MapKeyUp = (s, e) => { };

        #endregion

        #region Public Methods

        public void TurnOnSelectMode()
        {
            IsSelectMode = true;
        }

        public void ClearSelection()
        {
            NetworkManager.UnSelectContainers();
        }

        public void SelectOneContainer(NetElementContainer container)
        {
            NetworkManager.SelectOneContainer(container);
        }

        #endregion

        #region Helper Methods

        private MapPointEventArgs GetMapPointEventArgs(MouseEventArgs e)
        {
            var mapView = ((e.Source) as MapView);
            Debug.Assert(mapView != null, "mapView != null");

            Point screenPoint = e.GetPosition(mapView);
            MapPoint mapPoint = mapView.ScreenToLocation(screenPoint);

            return new MapPointEventArgs(screenPoint, mapPoint);
        }

        #endregion
    }
}