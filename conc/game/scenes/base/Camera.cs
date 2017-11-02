using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        int LevelWidth { get; set; }
        int LevelHeight { get; set; }

        void Update(GameTime gameTime);
        void Draw(SpriteBatch sb);

        void Initialize();
        void SetPosition(Vector2 position);
    }

    public class Camera : GameComponent, ICamera
    {
        private Vector2 _position;
        private float _viewportHeight;
        private float _viewportWidth;

        private readonly float _defaultViewportX = GameSettings.TargetWidth;
        private readonly float _defaultViewportY = GameSettings.TargetHeight;

        public Camera(Game game) : base(game)
        {
            _position = new Vector2(0, 0);
        }

        public Vector2 Position
        {
            get => _position;
            set
            {
                value.X = MathHelper.Clamp(_position.X, _viewportWidth / 2f / Scale.X, MapWidth - _viewportWidth / 2f / Scale.Y);
                value.Y = MathHelper.Clamp(_position.Y, _viewportHeight / 2f / Scale.X, MapHeight - _viewportHeight / 2f / Scale.Y);

                _position = value;
            }
        }

        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; }
        public Vector2 ScreenCenter { get; protected set; }
        public Matrix Transform { get; set; }
        public int LevelWidth { get; set; }
        public int LevelHeight { get; set; }
        public float Velocity { get; set; }
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }

        public void Draw(SpriteBatch sb)
        {
        }

        public override void Initialize()
        {
            _viewportWidth = Game.GraphicsDevice.Viewport.Width;
            _viewportHeight = Game.GraphicsDevice.Viewport.Height;

            ScreenCenter = new Vector2(_viewportWidth / 2, _viewportHeight / 2);

            var scaleX = _viewportWidth / _defaultViewportX;
            var scaleY = _viewportHeight / _defaultViewportY;

            Scale = new Vector2(scaleX, scaleY);
            Velocity = 100f;

            base.Initialize();
        }

        public void SetPosition(Vector2 position)
        {
            _position.X = position.X + _viewportWidth / 2f;
            _position.Y = position.Y + _viewportHeight / 2f;
        }

        public override void Update(GameTime gameTime)
        {
            Transform = Matrix.Identity *
                        Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                        Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                        Matrix.CreateScale(new Vector3(Scale.X, Scale.Y, 1f));

            Origin = ScreenCenter / Scale;

            base.Update(gameTime);
        }
    }
}