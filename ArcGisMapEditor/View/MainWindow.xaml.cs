using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Esri.ArcGISRuntime.Controls;

namespace ArcGisMapEditor.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MyMapView_LayerLoaded(object sender, LayerLoadedEventArgs e)
        {
            if (e.LoadError == null)
                return;

            Debug.WriteLine("Error while loading layer : {0} - {1}", e.Layer.ID, e.LoadError.Message);
        }

        private void MyMapView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Отключение создания окна приближения при нажатом Shift
            DisableZoomBox(sender, e);
        }

        private static void DisableZoomBox(object sender, MouseButtonEventArgs e)
        {
            if (!Keyboard.IsKeyDown(Key.LeftShift)) return;

            var args = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
            {
                RoutedEvent = Mouse.MouseUpEvent
            };
            ((UIElement)sender).RaiseEvent(args);  
        }
    }
}
