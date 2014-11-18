using System;

namespace ArcGisMapEditor.Model
{
    public class NetElement
    {
        public NetElement(NetElementType elementType)
        {
            Id = Guid.NewGuid();
            ElementType = elementType;
        }

        public Guid Id { get; private set; }
        public string Name { get; set; }
        public NetElementType ElementType { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public NetElement Copy()
        {
            var netElement = new NetElement(this.ElementType)
            {
                Name = this.Name,
                X = this.X,
                Y = this.Y
            };

            return netElement;
        }

        #region Equality members

		protected bool Equals(NetElement other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NetElement) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
 
	    #endregion  
    }
}