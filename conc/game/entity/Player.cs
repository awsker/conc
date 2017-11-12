using conc.game.entity.animation;
using conc.game.entity.baseclass;
using conc.game.entity.movement;
using conc.game.scenes;
using conc.game.util;
using Microsoft.Xna.Framework;
using tile.math;

namespace conc.game.entity
{
    public interface IPlayer : IAnimatedEntity, IMovingEntity
    {
    }

    public class Player : AnimatedEntity, IPlayer
    {
        private IGameScene _scene;
        private bool _onGround;
        private Line _currentGround;

        public Player(Vector2 position, IAnimator animator, IGameScene scene) : base(position, animator)
        {
            _scene = scene;
            CollisionSettings = new CollisionSettings(true, ActionOnCollision.PushOut, 0f, 1f);
        }
        
        public Vector2 Velocity { get; set; }
        public CollisionSettings CollisionSettings { get; }

        public void OnCollisionWithBackground(MovementHandler handler, Vector2 collision, Line line)
        {
            var lineAngle = Vector2.Dot(line.Vector.Normalized(), new Vector2(1f, 0f));
            if (lineAngle > 0.707)
            {
                _onGround = true;
                _currentGround = line;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
    }
}