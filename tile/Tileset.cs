using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace tile
{
    public interface ITileset
    {
        string Source { get; }
        int TileWidth { get; }
        int TileHeight { get; }
        int Columns { get; }
        int Rows { get; }
        int FirstGid { get; }
        int TileCount { get; }
        TileProperties[] Tiles { get; }
        Texture2D Texture { get; set; }

    }

    [DataContract(Name = "Tileset")]
    public class Tileset : ITileset
    {
        public Tileset(string source, int firstgid, int tilecount, int tileWidth, int tileHeight, int columns, int rows)
        {
            Source = source;
            FirstGid = firstgid;
            TileCount = tilecount;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            Columns = columns;
            Rows = rows;
            Tiles = new TileProperties[columns * rows];
        }
        
        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public int FirstGid { get; set; }
        [DataMember]
        public int TileCount { get; set; }
        [DataMember]
        public int TileWidth { get; set; }
        [DataMember]
        public int TileHeight { get; set; }
        [DataMember]
        public int Columns { get; set; }
        [DataMember]
        public int Rows { get; set; }
        public TileProperties[] Tiles { get; }
        public Texture2D Texture { get; set; }
    }

    public class TileProperties : Dictionary<string, string>{}
}
