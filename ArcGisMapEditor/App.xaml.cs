﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Windows;
using ArcGisMapEditor.View;
using ArcGisMapEditor.ViewModel;
using Esri.ArcGISRuntime;

namespace ArcGisMapEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void AppStartup(object sender, StartupEventArgs e)
        {
            SetupProxy();
            InitArcGIS();

            var view = new MainWindow();
            view.Show();
        }

        private static void SetupProxy()
        {
            var webProxy = WebRequest.GetSystemWebProxy();
            webProxy.Credentials = new NetworkCredential("Garanin_MS", ",tkrf");
            WebRequest.DefaultWebProxy = webProxy;
        }

        private void InitArcGIS()
        {
            try
            {
                // Deployed applications must be licensed at the Basic level or greater (https://developers.arcgis.com/licensing).
                // To enable Basic level functionality set the Client ID property before initializing the ArcGIS Runtime.
                ArcGISRuntimeEnvironment.ClientId = "0O4hL2fzzOTbaiE2";

                // Initialize the ArcGIS Runtime before any components are created.
                ArcGISRuntimeEnvironment.Initialize();

                // Standard level functionality can be enabled once the ArcGIS Runtime is initialized.                
                // To enable Standard level functionality you must either:
                // 1. Allow the app user to authenticate with ArcGIS Online or Portal for ArcGIS then call the set license method with their license info.
                // ArcGISRuntimeEnvironment.License.SetLicense(LicenseInfo object returned from an ArcGIS Portal instance)
                // 2. Call the set license method with a license string obtained from Esri Customer Service or your local Esri distributor.
                // ArcGISRuntimeEnvironment.License.SetLicense("<Your License String or Strings (extensions) Here>");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ArcGIS Runtime initialization failed.");

                // Exit application
                this.Shutdown();
            }
        }
    }
}
