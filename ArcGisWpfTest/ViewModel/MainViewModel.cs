using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ArcGisWpfTest.Helpers;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Symbols;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace ArcGisWpfTest.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public const string BingMapKey = "AuBdTBDeWwjN2GrEv8yhlye74nvY8YT58w5WUdBX5i59Y9eikQJLdEsKawTr4LlS";

        public const string SymbolsResourceDictionaryUri =
            @"pack://application:,,,/ArcGisWpfTest;component/Resources/SymbolsDictionary.xaml";

        private ResourceDictionary _resources;

        public MainViewModel()
        {
            CheckInternetConnection();

            InitResourceDictionary();
        }

        private void InitResourceDictionary()
        {
            _resources = new ResourceDictionary
            {
                Source = new Uri(SymbolsResourceDictionaryUri, UriKind.RelativeOrAbsolute)
            };
        }

        // Properties

        #region HasInternetConnection

        private bool _hasInternetConnection;

        public bool HasInternetConnection
        {
            get { return _hasInternetConnection; }
            set
            {
                if (Equals(value, _hasInternetConnection)) return;
                _hasInternetConnection = value;
                RaisePropertyChanged("HasInternetConnection");

                IsShowMap = value;
            }
        } 
        #endregion

        #region IsShowMap

        private bool _isShowMap;

        public bool IsShowMap
        {
            get { return _isShowMap; }
            set
            {
                if (Equals(value, _isShowMap)) return;
                _isShowMap = value;
                RaisePropertyChanged("IsShowMap");
            }
        }
        
        #endregion

        #region TestGraphics

        private ObservableCollection<Graphic> _testGraphics;

        public ObservableCollection<Graphic> TestGraphics
        {
            get { return _testGraphics; }
            set
            {
                if (Equals(value, _testGraphics)) return;
                _testGraphics = value;
                RaisePropertyChanged("TestGraphics");
            }
        } 
        #endregion
        

        // Commands

        #region CheckInternetConnection

        private RelayCommand _checkInternetConnectionCommand;

        public ICommand CheckInternetConnectionCommand
        {
            get
            {
                return _checkInternetConnectionCommand
                       ?? (_checkInternetConnectionCommand = new RelayCommand(CheckInternetConnection));
            }
        }

        private void CheckInternetConnection()
        {
            HasInternetConnection = InternetHelper.CheckForInternetConnectionHttp();
        }

        #endregion

        #region Test

        private RelayCommand _testCommand;

        public ICommand TestCommand
        {
            get
            {
                return _testCommand
                       ?? (_testCommand = new RelayCommand(Test));
            }
        }

        private void Test()
        {
            TestGraphics = new ObservableCollection<Graphic>();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    var graphic = new Graphic { Symbol = new SimpleMarkerSymbol() };
                    graphic.Symbol.ControlTemplate = ((SimpleMarkerSymbol)_resources["RedMarkerSymbol"]).ControlTemplate;
                    graphic.Geometry = new MapPoint(i * 100000, j * 200000);
                    TestGraphics.Add(graphic);
                }
            }
        }

        #endregion

    }
}