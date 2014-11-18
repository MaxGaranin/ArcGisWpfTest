using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Symbols;

namespace ArcGisWpfTest.View
{
    public partial class MainView : Window
    {
        public const string SymbolsResourceDictionaryUri =
            @"pack://application:,,,/ArcGisWpfTest;component/Resources/SymbolsDictionary.xaml";

        private ObservableCollection<Graphic> _testGraphics;
        private ResourceDictionary _resources;
        private Random _random;

        public MainView()
        {
            InitializeComponent();

            _random = new Random();

            _resources = new ResourceDictionary
            {
                Source = new Uri(SymbolsResourceDictionaryUri, UriKind.RelativeOrAbsolute)
            };
        }

        private void ShowMapCheckBox_OnClick(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox) sender;

            MapLayer.Visible = checkBox.IsChecked.Value;
        }

        private void Zoom()
        {
            MyMap.ZoomTo(new Envelope(-11000000, 4000000, -9000000, 6000000));
        }

        private void TestButton_OnClick(object sender, RoutedEventArgs e)
        {
            CreateLayer(MyMap, "Testlayer1", 0, 0);
            CreateLayer(MyMap, "Testlayer2", 5000, 5000);
            CreateLayer(MyMap, "Testlayer3", 6000, 6000);
            CreateLayer(MyMap, "Testlayer4", -10000, -10000);
            CreateLayer(MyMap, "Testlayer5", -20000, -20000);
        }

        private void CreateLayer(Map map, string layerID, int i0, int j0)
        {
            _testGraphics = new ObservableCollection<Graphic>();

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    var graphic = new Graphic { Symbol = GetRandomSymbol() };

                    // Красная анимация
                    // var graphic = new Graphic { Symbol = new SimpleMarkerSymbol() };
                    // graphic.Symbol.ControlTemplate = ((MarkerSymbol)_resources["ProblemsMarkerSymbol"]).ControlTemplate;

                    graphic.Geometry = new MapPoint(
                        i0 + i * 10000 * _random.Next(-50, 50), 
                        j0 + j * 20000 * _random.Next(-50, 50));

                    _testGraphics.Add(graphic);

                }
            }

            var testLayer = new GraphicsLayer { ID = layerID };
            map.Layers.Add(testLayer);

            testLayer.ClearGraphics();
            testLayer.Graphics.AddRange(_testGraphics);            
        }

        private Symbol GetRandomSymbol()
        {
            var value = _random.Next(3);
            string resourceName = "RedMarkerSymbol";

            switch (value)
            {
                case 0:
                    resourceName = "RedMarkerSymbol";
                    break;
                case 1:
                    resourceName = "GreenMarkerSymbol";
                    break;
                case 2:
                    resourceName = "BlueMarkerSymbol";
                    break;
            }
            return (SimpleMarkerSymbol) _resources[resourceName];
        }

        private void OpenTestViewButton_OnClick(object sender, RoutedEventArgs e)
        {
            var testView = new TestView();
            testView.Show();
        }
    }
}
