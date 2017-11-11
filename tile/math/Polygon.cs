using System.Runtime.Serialization;

namespace tile.math
{
    [DataContract(Name = "Polygon")]
    public class Polygon : IPolygon
    {
        private Line[] _lines;

        public Polygon(Line[] lines)
        {
            _lines = lines;
        }

        [DataMember]
        public Line[] Lines => _lines;
    }
}
