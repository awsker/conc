using System;
using Microsoft.Xna.Framework;

namespace tile.math
{
    public class RectangleF : IPolygon
    {
        public float MinX { get; }
        public float MinY { get; }
        public float MaxX { get; }
        public float MaxY { get; }
        public float Width => MaxX - MinX;
        public float Height => MaxY - MinY;

        public RectangleF(float p1x, float p1y, float p2x, float p2y)
        {
            MinX = Math.Min(p1x, p2x);
            MinY = Math.Min(p1y, p2y);
            MaxX = Math.Max(p1x, p2x);
            MaxY = Math.Max(p1y, p2y);
        }

        public RectangleF(Vector2 corner1, Vector2 corner2):this(corner1.X, corner1.Y, corner2.X, corner2.Y)
        { }

        public RectangleF(Rectangle rect) : this(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height)
        { }

        public Rectangle CreateRectangle()
        {
            return new Rectangle(Convert.ToInt32(MinX), Convert.ToInt32(MinY), Convert.ToInt32(Width), Convert.ToInt32(Height));
        }

        public static RectangleF BoundsFromLine(Line line)
        {
            float minx = Math.Min(line.Start.X, line.End.X);
            float miny = Math.Min(line.Start.Y, line.End.Y);
            float maxx = Math.Max(line.Start.X, line.End.X);
            float maxy = Math.Max(line.Start.Y, line.End.Y);
            if (line.LineType != LineType.BothCapped)
            {
                if (line.LineType == LineType.Infinite || line.End.X < line.Start.X)
                    minx = float.NegativeInfinity;
                if (line.LineType == LineType.Infinite || line.End.X > line.Start.X)
                    maxx = float.PositiveInfinity;
                if (line.LineType == LineType.Infinite || line.End.Y < line.Start.Y)
                    miny = float.NegativeInfinity;
                if (line.LineType == LineType.Infinite || line.End.Y > line.Start.Y)
                    maxy = float.PositiveInfinity;
            }
            
            return new RectangleF(minx, miny, maxx, maxy);
        }

        public bool Intersects(RectangleF other)
        {
            return MaxX >= other.MinX &&
                   MinX <= other.MaxX &&
                   MaxY >= other.MinY &&
                   MinY <= other.MinY;
        }

        public Line[] Lines => new[]
        {
            new Line(MinX, MinY, MaxX, MinY),
            new Line(MaxX, MinY, MaxX, MaxY),
            new Line(MaxX, MaxY, MinX, MaxY),
            new Line(MinX, MaxY, MinX, MinY),
        };
    }
}
