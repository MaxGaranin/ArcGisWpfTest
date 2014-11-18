using System;
using System.Windows;
using ArcGisMapEditor.Model;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;

namespace ArcGisMapEditor.ViewModel.Helpers
{
    public class ResourceHelper
    {
        public static ResourceDictionary GetSymbolsResources()
        {
            var resources = new ResourceDictionary
            {
                Source = new Uri(Constants.SymbolsResourceDictionaryUri, UriKind.RelativeOrAbsolute)
            };

            return resources;
        }

        public static Graphic CreateNetElementGraphic(NetElementType netElementType)
        {
            var resources = GetSymbolsResources();

            var resourceName = "";
            switch (netElementType)
            {
                case NetElementType.Well:
                    resourceName = "RedMarkerSymbol";
                    break;
                case NetElementType.Dns:
                    resourceName = "GreenMarkerSymbol";
                    break;
                case NetElementType.Upsv:
                    resourceName = "BlueMarkerSymbol";
                    break;
            }

            var graphic = new Graphic
            {
                Symbol = (SimpleMarkerSymbol)resources[resourceName]
            };

            return graphic;
        }
    }
}