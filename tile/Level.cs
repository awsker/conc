using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using tile.math;

namespace tile
{
    public interface ILevel
    {
        string Name { get; set; }
        ITileset[] Tilesets { get; set; }
        ITile[,] Tiles { get; set; }
        ITile[,] Background { get; set; }
        ITile[,] Foreground { get; set; }
        Line[] CollisionLines { get; set; }
        Rectangle[,] CameraCollisions { get; set; }
        IList<Rectangle> Deaths { get; set; }
        Rectangle Start { get; set; }
        Rectangle Goal { get; set; }
        IList<Rectangle> Checkpoints { get; set; }
        Vector2 ViewStart { get; set; }

        int Width { get; }
        int Height { get; }
        int TileWidth { get; }
        int TileHeight { get; }

        //IEnumerable<ITile> GetPotentialCollisionTiles(Rectangle boundingBox);
        IEnumerable<Line> GetPotentialCollisionLines(Line line);
    }

    [DataContract(Name = "Level")]
    public class Level : ILevel
    {
        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public ITileset[] Tilesets { get; set; }
       
        [DataMember]
        public ITile[][] TilesForSerialize { get; set; }
        public ITile[,] Tiles { get; set; }

        [DataMember]
        public ITile[][] BackgroundForSerialize { get; set; }
        public ITile[,] Background { get; set; }

        [DataMember]
        public ITile[][] ForegroundForSerialize { get; set; }
        public ITile[,] Foreground { get; set; }

        [DataMember]
        public Line[] CollisionLines { get; set; }
        public Rectangle[,] CameraCollisions { get; set; }

        [DataMember]
        public IList<Rectangle> Deaths { get; set; }

        [DataMember]
        public Rectangle Start { get; set; }

        [DataMember]
        public Rectangle Goal { get; set; }

        [DataMember]
        public IList<Rectangle> Checkpoints { get; set; }

        [DataMember]
        public Vector2 ViewStart { get; set; }

        public int Width => Tiles.GetLength(0);
        public int Height => Tiles.GetLength(1);

        [DataMember]
        public int TileWidth { get; set; }
        [DataMember]
        public int TileHeight { get; set; }

        public IEnumerable<Line> GetPotentialCollisionLines(Line line)
        {
            //Todo: TEMP!!!!
            return CollisionLines;
        }

        public IEnumerable<ITile> GetPotentialCollisionTiles(Rectangle boundingBox)
        {
            for (int x = Math.Max(0, boundingBox.X / TileWidth); x <= (boundingBox.X + boundingBox.Width) / TileWidth && x < Width; ++x)
                for (int y = Math.Max(0, boundingBox.Y / TileHeight); y <= (boundingBox.Y + boundingBox.Height) / TileHeight && x < Height; ++y)
                    yield return Tiles[x, y];
        }

        [OnSerializing]
        public void BeforeSerializing(StreamingContext ctx)
        {
            TilesForSerialize = SerializeTileArray(Tiles);
            BackgroundForSerialize = SerializeTileArray(Background);
            ForegroundForSerialize = SerializeTileArray(Foreground);
        }

        [OnDeserialized]
        public void AfterDeserializing(StreamingContext ctx)
        {
            Tiles = DeserializeTileArray(TilesForSerialize);
            Background = DeserializeTileArray(BackgroundForSerialize);
            Foreground = DeserializeTileArray(ForegroundForSerialize);
        }

        private ITile[][] SerializeTileArray(ITile[,] from)
        {
            var dimension0 = from.GetLength(0);
            var dimension1 = from.GetLength(1);
            var to = new ITile[dimension0][];
            for (var i = 0; i < dimension0; i++)
            {
                to[i] = new ITile[dimension1];
                for (var j = 0; j < dimension1; j++)
                {
                    to[i][j] = from[i, j];
                }
            }

            return to;
        }

        private ITile[,] DeserializeTileArray(ITile[][] from)
        {
            var dimension0 = from.Length;
            var dimension1 = from[0].Length;
            var to = new ITile[dimension0, dimension1];
            for (var i = 0; i < dimension0; i++)
            {
                for (var j = 0; j < dimension1; j++)
                {
                    to[i, j] = from[i][j];
                }
            }

            return to;
        }

    }
}
