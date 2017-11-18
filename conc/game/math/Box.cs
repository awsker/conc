using System.Linq;
using Microsoft.Xna.Framework;
using tile.math;

namespace conc.game.math
{
    public class Box : IPolygon
    {
        private Vector2[] _points;
        private float _width, _height;

        public Box(float width, float height)
        {
            _width = width;
            _height = height;
            _points = new Vector2[4];
            _points[0] = new Vector2(0f, 0f);
            _points[1] = new Vector2(width, 0f);
            _points[2] = new Vector2(width, height);
            _points[3] = new Vector2(0f, height);
            Scale = Vector2.One;
            Rotation = 0f;
        }

        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public Vector2 Origin { get; set; }
        public float Rotation { get; set; }
        public float Width => _width;
        public float Height => _height;
        public Rectangle Rectangle => new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height);

        private Line[] calculateLines()
        {
            var transform = Matrix.Identity *
                            Matrix.CreateTranslation(-Origin.X, -Origin.Y, 0) *
                            Matrix.CreateRotationZ(Rotation) *
                            Matrix.CreateScale(new Vector3(Scale.X, Scale.Y, 1f)) *
                            Matrix.CreateTranslation(Position.X, Position.Y, 0);
                            
            Vector2[] activePoints = _points.Select(p => Vector2.Transform(p, transform)).ToArray();
            Line[] lines = new Line[activePoints.Length];
            for (int i = 0; i < activePoints.Length; ++i)
            {
                var cp = activePoints[i];
                var np = activePoints[(i + 1) % activePoints.Length];
                lines[i] = new Line(cp, np);
            }
            return lines;
        }

        public Line[] Lines => calculateLines();
    }
}
