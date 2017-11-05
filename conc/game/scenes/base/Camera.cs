using conc.game.entity.baseclass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tile;

namespace conc.game.scenes.@base
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
    }

    public class Camera : ICamera
    {
        private GraphicsDeviceManager _device;

        private readonly float _defaultViewportX = GameSettings.TargetWidth;
        private readonly float _defaultViewportY = GameSettings.TargetHeight;

        private IEntity _target;

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
        }

        public void Update(GameTime gameTime)
        {
            if (_target != null)
            {
                SetPosition(_target.Transform.Position);
            }
            var viewportWidth = _device.PreferredBackBufferWidth;
            var viewportHeight = _device.PreferredBackBufferHeight;

            var screenCenter = new Vector2(viewportWidth / 2f, viewportHeight / 2f);

            var scaleX = viewportWidth / _defaultViewportX;
            var scaleY = viewportHeight / _defaultViewportY;

            var innerScale = new Vector2(scaleX, scaleY);
            var totalScale = innerScale * Scale;
            
            var origin = screenCenter / totalScale;

            Transform = Matrix.Identity *
                        Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                        Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateTranslation(origin.X, origin.Y, 0) *
                        Matrix.CreateScale(new Vector3(totalScale.X, totalScale.Y, 1f));
        }
    }
}