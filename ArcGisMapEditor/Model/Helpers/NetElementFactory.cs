using System.Collections.Generic;

namespace ArcGisMapEditor.Model.Helpers
{
    public class NetElementFactory
    {
        private static readonly IDictionary<NetElementType, NetElement> TypeToElements;

        static NetElementFactory()
        {
            TypeToElements = new Dictionary<NetElementType, NetElement>
            {
                {NetElementType.Empty, new NetElement(NetElementType.Empty)},
                {NetElementType.Well, new NetElement(NetElementType.Well)},
                {NetElementType.Upsv, new NetElement(NetElementType.Upsv)},
                {NetElementType.Dns, new NetElement(NetElementType.Dns)},
                {NetElementType.Pipe, new NetElement(NetElementType.Pipe)},
            };
        }

        public static NetElement CreateNetElement(NetElementType netElementType)
        {
            return TypeToElements[netElementType];
        }
    }
}