using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
        Point Start { get; set; }
        Rectangle Goal { get; set; }
        IList<Checkpoint> Checkpoints { get; set; }
        Vector2 ViewStart { get; set; }

        int Width { get; }
        int Height { get; }
        int TileWidth { get; }
        int TileHeight { get; }

        void LoadContent(GraphicsDevice device, ContentManager manager);
        void DrawLevel(GraphicsDevice device);
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
        public Point Start { get; set; }

        [DataMember]
        public Rectangle Goal { get; set; }

        [DataMember]
        public IList<Checkpoint> Checkpoints { get; set; }

        [DataMember]
        public Vector2 ViewStart { get; set; }

        public int Width => Tiles.GetLength(0);
        public int Height => Tiles.GetLength(1);

        [DataMember]
        public int TileWidth { get; set; }
        [DataMember]
        public int TileHeight { get; set; }

        private List<BufferData> _buffers = null;

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

        public void LoadContent(GraphicsDevice device, ContentManager contentManager)
        {
            var tilesetPath = @"tilesets\";
            foreach(var tileset in Tilesets)
            {
                tileset.Texture = contentManager.Load<Texture2D>(tilesetPath + tileset.Source);
            }
            calculateVertexBuffers(device);
        }

        public void DrawLevel(GraphicsDevice device)
        {
            drawBuffers(device);
        }

        private void calculateVertexBuffers(GraphicsDevice device)
        {
            if (_buffers != null) //Only create buffers once
                return;

            _buffers = new List<BufferData>();
            _buffers.AddRange(createVertexBuffer(device, Background));
            _buffers.AddRange(createVertexBuffer(device, Tiles));
            _buffers.AddRange(createVertexBuffer(device, Foreground));
        }

        private void drawBuffers(GraphicsDevice device)
        {
            if (_buffers == null)
                calculateVertexBuffers(device);

            device.RasterizerState = new RasterizerState() { CullMode = CullMode.None };
            device.DepthStencilState = DepthStencilState.None;

            foreach (var buffer in _buffers)
            {
                device.Indices = buffer.IndexBuffer;
                device.SetVertexBuffer(buffer.VertexBuffer);
                device.Textures[0] = Tilesets[buffer.TilesetIndex].Texture;
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, buffer.IndexBuffer.IndexCount / 3);
            }
        }

        private IEnumerable<BufferData> createVertexBuffer(GraphicsDevice device, ITile[,] tiles)
        {
            var xsize = tiles.GetLength(0);
            var ysize = tiles.GetLength(1);

            for (int tsi = 0; tsi < Tilesets.Length; ++tsi)
            {
                var tileset = Tilesets[tsi];
                var vbData = new List<VertexPositionTexture>();
                var ibData = new List<int>();
                bool found = false;
                for (int x = 0; x < xsize; ++x)
                {
                    for (int y = 0; y < ysize; ++y)
                    {
                        var tile = tiles[x, y];
                        if (tile?.TilesetIndex == tsi)
                        {
                            found = true;
                            var fx = x * tileset.TileWidth;
                            var fy = y * tileset.TileHeight;
                            var fx2 = (x + 1) * tileset.TileWidth;
                            var fy2 = (y + 1) * tileset.TileHeight;
                            var currentFbindex = vbData.Count;
                            var sourceRect = tile.Source;
                            float tsWidth = tileset.TileWidth * tileset.Columns;
                            float tsHeight = tileset.TileHeight * tileset.Rows;
                            var tx = sourceRect.X / tsWidth;
                            var ty = sourceRect.Y / tsHeight;
                            var tx2 = (sourceRect.X + sourceRect.Width) / tsWidth;
                            var ty2 = (sourceRect.Y + sourceRect.Height) / tsHeight;

                            vbData.Add(new VertexPositionTexture(new Vector3(fx, fy, 0f), new Vector2(tx, ty)));
                            vbData.Add(new VertexPositionTexture(new Vector3(fx2, fy, 0f), new Vector2(tx2, ty)));
                            vbData.Add(new VertexPositionTexture(new Vector3(fx2, fy2, 0f), new Vector2(tx2, ty2)));
                            vbData.Add(new VertexPositionTexture(new Vector3(fx, fy2, 0f), new Vector2(tx, ty2)));
                            ibData.AddRange(new[] { currentFbindex, currentFbindex + 3, currentFbindex + 1, currentFbindex + 3, currentFbindex + 1, currentFbindex + 2 });
                        }
                    }
                }
                if (found)
                {
                    var vb = new VertexBuffer(device, VertexPositionTexture.VertexDeclaration, vbData.Count, BufferUsage.WriteOnly);
                    vb.SetData(vbData.ToArray());
                    var ib = new IndexBuffer(device, IndexElementSize.ThirtyTwoBits, ibData.Count, BufferUsage.WriteOnly);
                    ib.SetData(ibData.ToArray());
                    yield return new BufferData(vb, tsi, ib);
                }
            }
        }
    }
}
