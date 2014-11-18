using System.ComponentModel;
using ArcGisMapEditor.Model;
using ArcGisMapEditor.ViewModel.Helpers;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;

namespace ArcGisMapEditor.ViewModel.Containers
{
    public class NetElementContainer : INotifyPropertyChanged
    {
        public NetElementContainer(NetElement netElement)
        {
            NetElement = netElement;

            Graphic = ResourceHelper.CreateNetElementGraphic(netElement.ElementType);
            Graphic.Attributes[Constants.NetElementContainerGraphicKey] = this;

            MapPoint = GeometryHelper.CreateNetElementMapPoint(netElement);
        }

        #region Properties

        public NetElement NetElement { get; set; }
        public Graphic Graphic { get; set; }

        #region MapPoint

        private MapPoint _mapPoint;

        public MapPoint MapPoint
        {
            get { return _mapPoint; }
            set
            {
                // if (Equals(value, _mapPoint)) return;
                _mapPoint = value;
                RaisePropertyChanged("MapPoint");

                NetElement.X = value.X;
                NetElement.Y = value.Y;
                Graphic.Geometry = value;
            }
        }

        #endregion

        #region IsSelected

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (Equals(value, _isSelected)) return;
                _isSelected = value;
                RaisePropertyChanged("IsSelected");

                Graphic.IsSelected = value;
            }
        }

        #endregion

        #region IsVisible

        private bool _isVisible;

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                RaisePropertyChanged("IsVisible");

                Graphic.IsVisible = value;
            }
        }

        #endregion

        #endregion

        #region Public methods

        public NetElementContainer Copy()
        {
            var netElement = this.NetElement.Copy();
            var container = new NetElementContainer(netElement)
            {
                Graphic = new Graphic
                {
                    Symbol = this.Graphic.Symbol,
                    Geometry = this.Graphic.Geometry,
                }
            };
            container.Graphic.CopyFrom(this.Graphic.Attributes);

            return container;
        }

        #endregion

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}