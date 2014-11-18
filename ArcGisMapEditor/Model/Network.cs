using System.Collections.Generic;

namespace ArcGisMapEditor.Model
{
    public class Network
    {
        public Network()
        {
            NetElements = new List<NetElement>();
        }

        public IList<NetElement> NetElements { get; set; } 
    }
}