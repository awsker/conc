using System;
using conc.game.entity.baseclass;
using conc.game.entity.movement;
using conc.game.extensions;
using Microsoft.Xna.Framework;
using tile.math;

namespace conc.game.entity
{
    public class NinjaRope : Entity
    {
        private IMovingEntity _owner;

        private float _distance;
        private float _velocity;
        private float _gravity;

        private bool _retracting;
        
        public NinjaRope(Vector2 position, IMovingEntity owner, LookDirection spinDirection, float gravity)
        {
            Position = position;

            _owner = owner;
            _gravity = gravity;
            _distance = distanceToOwner();
            _velocity = Math.Max(owner.Velocity.Length() * 1.03f, 150f) * (spinDirection == LookDirection.Right ? -1 : 1);
        }

        public override void Update(GameTime gameTime)
        {
            if (!_retracting)
            {
                var line = new Line(_owner.Transform.Position, Position);
                var lineNormal = line.Normal;
                _velocity -= (float)(Math.Sin(Math.Atan2(-lineNormal.Y, lineNormal.X)) * _gravity * gameTime.ElapsedGameTime.TotalSeconds);

                if (line.Length > _distance)
                {
                    _owner.Transform.Position = Position + line.Vector.Normalized() * -_distance;
                }
                _owner.Velocity = line.Normal * _velocity;
            }
        }

        public void Retract()
        {
            _retracting = true;
        }

        private float distanceToOwner()
        {
            return (Position - _owner.Transform.Position).Length();
        }

    }
}
