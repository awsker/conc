﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace tile
{
    public interface ILevel
    {
        string Name { get; set; }
        string Tileset { get; set; }
        ITile[,] Tiles { get; set; }
        ITile[,] Background { get; set; }
        ITile[,] Foreground { get; set; }
        ICollisionTile[,] Collisions { get; set; }
        Rectangle[,] CameraCollisions { get; set; }
        IList<Rectangle> Deaths { get; set; }
        Vector2 Start { get; set; }
        Vector2 ViewStart { get; set; }

        int Width { get; }
        int Height { get; }
    }

    [DataContract(Name = "Level")]
    public class Level : ILevel
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Tileset { get; set; }

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
        public ICollisionTile[][] CollisionsForSerialize { get; set; }
        public ICollisionTile[,] Collisions { get; set; }
        public Rectangle[,] CameraCollisions { get; set; }

        [DataMember]
        public IList<Rectangle> Deaths { get; set; }

        [DataMember]
        public Vector2 Start { get; set; }

        [DataMember]
        public Vector2 ViewStart { get; set; }

        public int Width => Tiles.GetLength(0);
        public int Height => Tiles.GetLength(1);

        [OnSerializing]
        public void BeforeSerializing(StreamingContext ctx)
        {
            TilesForSerialize = SerializeTileArray(Tiles);
            BackgroundForSerialize = SerializeTileArray(Background);
            ForegroundForSerialize = SerializeTileArray(Foreground);
            CollisionsForSerialize = SerializeCollisionArray(Collisions);
        }

        [OnDeserialized]
        public void AfterDeserializing(StreamingContext ctx)
        {
            Tiles = DeserializeTileArray(TilesForSerialize);
            Background = DeserializeTileArray(BackgroundForSerialize);
            Foreground = DeserializeTileArray(ForegroundForSerialize);
            Collisions = DeserializeCollisionArray(CollisionsForSerialize);
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

        private ICollisionTile[][] SerializeCollisionArray(ICollisionTile[,] from)
        {
            var dimension0 = from.GetLength(0);
            var dimension1 = from.GetLength(1);
            var to = new ICollisionTile[dimension0][];
            for (var i = 0; i < dimension0; i++)
            {
                to[i] = new ICollisionTile[dimension1];
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

        private ICollisionTile[,] DeserializeCollisionArray(ICollisionTile[][] from)
        {
            var dimension0 = from.Length;
            var dimension1 = from[0].Length;
            var to = new ICollisionTile[dimension0, dimension1];
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