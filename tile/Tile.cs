using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace tile
{
    public interface ITile
    {
        int X { get; set; }
        int Y { get; set; }
        int TilesetIndex { get; set; }
        Rectangle Source { get; set; }
        Slope Slope { get; set; }
        Point[] Corners { get; }
    }

    [DataContract(Name = "Tile")]
    public class Tile : ITile
    {
        public Tile(int x, int y, int tilesetIndex, Rectangle source)
        {
            X = x;
            Y = y;
            TilesetIndex = tilesetIndex;
            Source = source;
        }

        [DataMember]
        public int X { get; set; }

        [DataMember]
        public int Y { get; set; }

        [DataMember]
        public int TilesetIndex { get; set; }

        [DataMember]
        public Rectangle Source { get; set; }

        [DataMember]
        public Slope Slope { get; set; }

        [DataMember]
        public Point[] Corners => getCorners().ToArray();

        private IEnumerable<Point> getCorners()
        {
            var x = Source.X;
            var y = Source.Y;
            var x2 = x + Source.Width;
            var y2 = y + Source.Height;

            if (Slope != Slope.FloorUp)
                yield return new Point(x, y);
            if (Slope != Slope.FloorDown)
                yield return new Point(x2, y);
            if (Slope != Slope.RoofUp)
                yield return new Point(x2, y2);
            if (Slope != Slope.RoofDown)
                yield return new Point(x, y2);
        }
    }

    [DataContract]
    public enum Slope
    {
        [EnumMember]
        None,
        [EnumMember]
        FloorDown,
        [EnumMember]
        FloorUp,
        [EnumMember]
        RoofDown,
        [EnumMember]
        RoofUp
    }
}
