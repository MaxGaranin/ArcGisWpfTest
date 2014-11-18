namespace ArcGisMapEditor.Model.Helpers
{
    public class NetworkHelper
    {
        public static Network GenerateNetwork()
        {
            var network = new Network();

            var netElement = new NetElement(NetElementType.Well)
            {
                Name = "Скважина 1",
                X = -5000000.0,
                Y = -1000000.0
            };
            network.NetElements.Add(netElement);

            netElement = new NetElement(NetElementType.Dns)
            {
                Name = "ДНС 1",
                X = 115000.0,
                Y = 505000.0
            };
            network.NetElements.Add(netElement);

            netElement = new NetElement(NetElementType.Upsv)
            {
                Name = "УПСВ 1",
                X = 1500000.0,
                Y = 2500000.0
            };
            network.NetElements.Add(netElement);

            return network;
        }
    }
}