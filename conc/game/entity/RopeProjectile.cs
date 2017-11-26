using conc.game.entity.baseclass;
using conc.game.entity.movement;
using conc.game.extensions;
using Microsoft.Xna.Framework;
using tile.math;

namespace conc.game.entity
{
    public class RopeProjectile : ImageEntity, IMovingEntity
    {
        public Vector2 Velocity { get; set; }

        private const float Speed = 800f;
        
        private IMovingEntity _owner;
        private LookDirection _direction;

        private NinjaRope _ropeEntity;

        private bool _retracting;

        public RopeProjectile(IMovingEntity owner, LookDirection direction):base(1, 1, new Point(2, 2), "trash/ropeprojectile")
        {
            _owner = owner;
            CollisionSettings = new CollisionSettings(true, ActionOnCollision.PushOut, 0f, 0f);

            _direction = direction;
            Position = owner.Transform.Position;
            Velocity = new Vector2(Speed * (direction == LookDirection.Left ? -1f : 1f), -Speed);
        }

        public void OnCollisionWithBackground(IMovementHandler handler, Vector2 collision, Line line)
        {
            Velocity = Vector2.Zero;
            CollisionSettings.CheckCollisionsWithBackground = false;
            if (_ropeEntity == null && !_retracting)
            {
                _ropeEntity = new NinjaRope(Position, _owner, _direction);
                Scene.AddEntity(_ropeEntity);
            }
        }

        public CollisionSettings CollisionSettings { get; }

        public override void Update(GameTime gameTime)
        {
            if (_retracting)
            {
                if ((Position - _owner.Transform.Position).Length() < 30f)
                    Destroy();
                else
                    Velocity = (_owner.Transform.Position - Position).Normalized() * Speed * 1.5f;
            }
        }

        public void Retract()
        {
            _retracting = true;
            _ropeEntity?.Retract();
            CollisionSettings.CheckCollisionsWithBackground = false;
        }

        public override void Destroy()
        {
            base.Destroy();
            _ropeEntity?.Destroy();
        }

        public bool IsHooked()
        {
            return _ropeEntity != null && !_retracting;
        }
    }
}
