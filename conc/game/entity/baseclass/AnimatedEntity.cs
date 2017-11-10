using conc.game.entity.animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace conc.game.entity.baseclass
{
    public interface IAnimatedEntity : IEntity
    {
    }

    public abstract class AnimatedEntity : Entity, IAnimatedEntity
    {
        private Texture2D _texture;

        private readonly IAnimator _animator;

        private int _frameIndex;

        private float _frameTimer;
        private AnimationType _currentAnimation;
        private IAnimationFrame _currentFrame;

        protected AnimatedEntity(Vector2 position, IAnimator animator) : base(position)
        {
            _animator = animator;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            _texture = contentManager.Load<Texture2D>("trash/" + _animator.SpriteSheet);
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad1))
                _currentAnimation = AnimationType.Idle;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
                _currentAnimation = AnimationType.Run;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad3))
                _currentAnimation = AnimationType.Jump;

            _frameTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_frameTimer >= 500f)
            {
                _frameTimer = 0f;
                _frameIndex++;
            }
            
            _animator.Animations.TryGetValue(_currentAnimation, out var animation);
            if (animation != null)
            {
                if (_frameIndex >= animation.Frames.Length)
                    _frameIndex = 0;

                _currentFrame = animation.Frames[_frameIndex];
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var sourceRectangle = new Rectangle(
                _currentFrame.X * _animator.FrameWidth,
                _currentFrame.Y * _animator.FrameHeight,
                _animator.FrameWidth,
                _animator.FrameHeight);

            var origin = new Vector2(_currentFrame.OffsetX, _currentFrame.OffsetY);

            spriteBatch.Draw(_texture, Transform.Position, sourceRectangle, Color.White, Transform.Rotation, origin, Transform.Scale, SpriteEffects.None, 0f);
        }
    }
}