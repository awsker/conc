using System;
using conc.game.entity.baseclass;
using conc.game.math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tile;
using tile.math;

namespace conc.game.scenes.baseclass
{
    public interface ICamera
    {
        Vector2 Position { get; set; }
        float Rotation { get; set; }
        Vector2 Scale { get; set; }
        Matrix Transform { get; }
        
        void Update(GameTime gameTime);
        void Draw(SpriteBatch sb);

        void SetPosition(Vector2 position);
        void SetTarget(IEntity target);

        void SetLevel(ILevel level);

        RectangleF GetViewRectangle();
    }

    public class Camera : ICamera
    {
        private readonly GraphicsDeviceManager _device;

        private readonly float _defaultViewportX = GameSettings.TargetWidth;
        private readonly float _defaultViewportY = GameSettings.TargetHeight;

        private IEntity _target;

        private ILevel _level;

        private float m_cameraSpeed = 20f;

        private RectangleF _viewingRectangle;
        

        public Camera(GraphicsDeviceManager device)
        {
            _device = device;
            Position = new Vector2(0, 0);
            Scale = new Vector2(1f, 1f);
        }

        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; }
        public Matrix Transform { get; set; }

        public void Draw(SpriteBatch sb)
        {
        }

        public void SetPosition(Vector2 position)
        {
            var pos = new Vector2(position.X, position.Y);
            Position = pos;
        }

        public void SetTarget(IEntity target)
        {
            _target = target;
            Position = _target.Transform.Position;
        }

        public void SetLevel(ILevel level)
        {
            _level = level;
        }
        
        public bool KeepCameraInsideBounds { get; set; }

        public void Update(GameTime gameTime)
        {
            if (_target != null)
                Position = Vector2.Lerp(Position, _target.Transform.Position, (float) gameTime.ElapsedGameTime.TotalSeconds * m_cameraSpeed);
            
            var viewportWidth = _device.PreferredBackBufferWidth;
            var viewportHeight = _device.PreferredBackBufferHeight;

            var worldViewportWidth = _defaultViewportX;
            var worldViewportHeight = _defaultViewportY;

            if (KeepCameraInsideBounds)
            {
                bool shrunk = false;
                if (worldViewportWidth > _level.Width * _level.TileWidth)
                {
                    worldViewportWidth = _level.Width * _level.TileWidth;
                    shrunk = true;
                }
                if (worldViewportHeight > _level.Height * _level.TileHeight)
                {
                    worldViewportHeight = _level.Height * _level.TileHeight;
                    shrunk = true;
                }
                if (shrunk)
                {
                    var ratio = (double)viewportWidth / (double)viewportHeight;
                    var worldRatio = (double) worldViewportWidth / (double) worldViewportHeight;
                    if (worldRatio > ratio)
                    {
                        worldViewportWidth = (float) (worldViewportHeight * ratio);
                    } else if (worldRatio < ratio)
                    {
                        worldViewportHeight = (float) (worldViewportWidth / ratio);
                    }
                }
                var minX = worldViewportWidth * 0.5f;
                var minY = worldViewportHeight * 0.5f;
                var maxX = _level.Width * _level.TileWidth - minX;
                var maxY = _level.Height * _level.TileHeight - minY;

                var x = Math.Max(minX, Math.Min(maxX, Position.X));
                var y = Math.Max(minY, Math.Min(maxY, Position.Y));
                Position = new Vector2(x, y);
            }

            var screenCenter = new Vector2(viewportWidth * 0.5f, viewportHeight * 0.5f);
            
            var scaleX = viewportWidth / worldViewportWidth;
            var scaleY = viewportHeight / worldViewportHeight;


            var innerScale = new Vector2(scaleX, scaleY);
            var totalScale = innerScale * Scale;
            
            var origin = screenCenter / totalScale;

            Transform = Matrix.Identity *
                        Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                        Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateTranslation(origin.X, origin.Y, 0) *
                        Matrix.CreateScale(new Vector3(totalScale.X, totalScale.Y, 1f));

            
            _viewingRectangle = new RectangleF(Position.X - worldViewportWidth * 0.5f, Position.Y - worldViewportHeight * 0.5f,Position.X + worldViewportWidth * 0.5f, Position.Y + worldViewportHeight * 0.5f);
        }

        public RectangleF GetViewRectangle()
        {
            return _viewingRectangle;
        }
    }
}