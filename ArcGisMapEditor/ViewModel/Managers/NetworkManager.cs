using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using ArcGisMapEditor.Model;
using ArcGisMapEditor.ViewModel.Containers;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Layers;

namespace ArcGisMapEditor.ViewModel.Managers
{
    public class NetworkManager : INotifyPropertyChanged
    {
        private readonly Network _network;
        private readonly Map _map;

        #region Constructor

        public NetworkManager(Network network, Map map)
        {
            _network = network;
            _map = map;

            NetElementContainers = CreateNetElementContainers(network.NetElements);
        }

        private ObservableCollection<NetElementContainer> CreateNetElementContainers(IEnumerable<NetElement> netElements)
        {
            var containers = new ObservableCollection<NetElementContainer>();

            foreach (var netElement in netElements)
            {
                var container = CreateNetElementContainer(netElement);
                containers.Add(container);
            }

            return containers;
        }

        private NetElementContainer CreateNetElementContainer(NetElement netElement)
        {
            var container = new NetElementContainer(netElement);
            PutNetElementContainerOnLayer(container);

            return container;
        }

        private void PutNetElementContainerOnLayer(NetElementContainer container)
        {
            var layer = (GraphicsLayer)_map.Layers
                .Single(x => x.ID == container.NetElement.ElementType.ToString());

            layer.Graphics.Add(container.Graphic);
        } 

        #endregion

        #region Properties

        #region NetElementContainers

        private ObservableCollection<NetElementContainer> _netElementContainers;

        public ObservableCollection<NetElementContainer> NetElementContainers
        {
            get { return _netElementContainers; }
            set
            {
                if (Equals(value, _netElementContainers)) return;
                _netElementContainers = value;
                RaisePropertyChanged("NetElementContainers");
            }
        }

        #endregion 

        #endregion

        #region Methods

        public void AddContainer(NetElementContainer container)
        {
            _network.NetElements.Add(container.NetElement);
            NetElementContainers.Add(container);

            var grahicsLayer = (GraphicsLayer)_map.Layers[container.NetElement.ElementType.ToString()];
            grahicsLayer.Graphics.Add(container.Graphic);
        }
        
        public void RemoveContainer(NetElementContainer container)
        {
            _network.NetElements.Remove(container.NetElement);
            NetElementContainers.Remove(container);

            var grahicsLayer = (GraphicsLayer) _map.Layers[container.NetElement.ElementType.ToString()];
            grahicsLayer.Graphics.Remove(container.Graphic);
        }

        public void SelectOneContainer(NetElementContainer container)
        {
            UnSelectContainers();
            container.IsSelected = true;
        }

        public IList<NetElementContainer> GetSelectedContainers()
        {
            return NetElementContainers.Where(c => c.IsSelected).ToList();
        }

        public void UnSelectContainers()
        {
            var containers = GetSelectedContainers();
            foreach (var container in containers)
            {
                container.IsSelected = false;
            }
        }

        public void RemoveSelectedElements()
        {
            var containers = GetSelectedContainers();
            foreach (var container in containers)
            {
                RemoveContainer(container);
            }
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