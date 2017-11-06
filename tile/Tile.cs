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
    }
}
