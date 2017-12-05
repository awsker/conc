using Microsoft.Xna.Framework.Graphics;

namespace tile
{
    struct BufferData
    {
        public VertexBuffer VertexBuffer;
        public int TilesetIndex;
        public IndexBuffer IndexBuffer;

        public BufferData(VertexBuffer vb, int tilesetIndex, IndexBuffer ib)
        {
            VertexBuffer = vb;
            TilesetIndex = tilesetIndex;
            IndexBuffer = ib;
        }
    }
}
