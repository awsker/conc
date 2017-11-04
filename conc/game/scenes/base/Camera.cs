using conc.game.entity.@base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tile;

namespace conc.game.scenes.@base
{
    public interface ICamera
    {
        Vector2 Position { get; set; }
        float Velocity { get; set; }
        float Rotation { get; set; }
        Vector2 Origin { get; }
        Vector2 Scale { get; set; }
        Vector2 ScreenCenter { get; }
        Matrix Transform { get; }

        int LevelWidth { get; }
        int LevelHeight { get; }

        void Update(GameTime gameTime);
        void Draw(SpriteBatch sb);

        void SetPosition(Vector2 position);
        void SetTarget(IEntity target);
    }

    public class Camera : ICamera
    {
        private Vector2 _position;
        private float _viewportHeight;
        private float _viewportWidth;

        private readonly float _defaultViewportX = GameSettings.TargetWidth;
        private readonly float _defaultViewportY = GameSettings.TargetHeight;
        //private readonly float _defaultViewportX = GameSettings.PreferredBackBufferWidth;
        //private readonly float _defaultViewportY = GameSettings.PreferredBackBufferHeight;

        private IEntity _target;

        public Camera(ILevel level)
        {
            _position = new Vector2(0, 0);
            LevelWidth = level.Width * GameSettings.TileSize;
            LevelHeight = level.Height * GameSettings.TileSize;

            _viewportWidth = GameSettings.PreferredBackBufferWidth;
            _viewportHeight = GameSettings.PreferredBackBufferHeight;

            ScreenCenter = new Vector2(_viewportWidth / 2, _viewportHeight / 2);

            var scaleX = _viewportWidth / _defaultViewportX;
            var scaleY = _viewportHeight / _defaultViewportY;

            Scale = new Vector2(scaleX, scaleY);
            Velocity = 1f;
        }

        public Vector2 Position
        {
            get => _position;
            set
            {
                //value.X = MathHelper.Clamp(_position.X, _viewportWidth / 2f / Scale.X, LevelWidth - _viewportWidth / 2f / Scale.Y);
                //value.Y = MathHelper.Clamp(_position.Y, _viewportHeight / 2f / Scale.X, LevelHeight - _viewportHeight / 2f / Scale.Y);

                _position = value;
            }
        }

        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; }
        public Vector2 ScreenCenter { get; protected set; }
        public Matrix Transform { get; set; }
        public int LevelWidth { get; }
        public int LevelHeight { get; }
        public float Velocity { get; set; }

        public void Draw(SpriteBatch sb)
        {
        }

        public void SetPosition(Vector2 position)
        {
            _position.X = position.X + _viewportWidth / 2f;
            _position.Y = position.Y + _viewportHeight / 2f;
        }

        public void SetTarget(IEntity target)
        {
            _target = target;
        }

        public void Update(GameTime gameTime)
        {
            //if (Keyboard.GetState().IsKeyDown(Keys.Up))
            //    _position.Y -= (float) gameTime.ElapsedGameTime.TotalMilliseconds * Velocity;
            //if (Keyboard.GetState().IsKeyDown(Keys.Down))
            //    _position.Y += (float) gameTime.ElapsedGameTime.TotalMilliseconds * Velocity;
            //if (Keyboard.GetState().IsKeyDown(Keys.Left))
            //    _position.X -= (float) gameTime.ElapsedGameTime.TotalMilliseconds * Velocity;
            //if (Keyboard.GetState().IsKeyDown(Keys.Right))
            //    _position.X += (float) gameTime.ElapsedGameTime.TotalMilliseconds * Velocity;

            if (_target != null)
            {
                //SetPosition(_target.Transform.Position);
                _position.X += (_target.Transform.Position.X - Position.X) * (float)gameTime.ElapsedGameTime.TotalMilliseconds * .05f;
                _position.Y += (_target.Transform.Position.Y - Position.Y) * (float)gameTime.ElapsedGameTime.TotalMilliseconds * .05f;
            }

            Transform = Matrix.Identity *
                        Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                        Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                        Matrix.CreateScale(new Vector3(Scale.X, Scale.Y, 1f));

            Origin = ScreenCenter / Scale;
        }
    }
}