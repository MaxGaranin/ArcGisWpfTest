using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Windows;
using ArcGisWpfTest.View;
using ArcGisWpfTest.ViewModel;

namespace ArcGisWpfTest
{
    public partial class App : Application
    {
        private void AppStartup(object sender, StartupEventArgs e)
        {
            SetupProxy();

            var viewModel = new MainViewModel();
            var view = new MainView();
            view.DataContext = viewModel;
            view.Show();            
        }

        /// <summary>
        /// Настройка доступа к прокси-серверу
        /// </summary>
        private static void SetupProxy()
        {
            var webProxy = WebRequest.GetSystemWebProxy();
            webProxy.Credentials = new NetworkCredential("Garanin_MS", ",tkrf");
            WebRequest.DefaultWebProxy = webProxy;
        }
    }
}
