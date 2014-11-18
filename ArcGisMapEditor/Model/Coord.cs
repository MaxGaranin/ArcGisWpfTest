namespace ArcGisMapEditor.Model
{
    public class Coord
    {
        public Coord()
        {
        }

        public Coord(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }

        #region ToString

        public override string ToString()
        {
            return string.Format("{0:F0} : {1:F0}", X, Y);
        } 

        #endregion

        #region Equality members

		protected bool Equals(Coord other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Coord) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode()*397) ^ Y.GetHashCode();
            }
        }
 
	    #endregion    
    }
}