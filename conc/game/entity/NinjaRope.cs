using System;
using conc.game.entity.baseclass;
using conc.game.entity.movement;
using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using tile.math;

namespace conc.game.entity
{
    public class NinjaRope : Entity
    {
        public override int Width => 0;
        public override int Height => 0;

        private IMovingEntity _owner;
        private LookDirection _direction;

        private float _distance;
        private float _velocity;

        public NinjaRope(Vector2 position, IMovingEntity owner, LookDirection spinDirection)
        {
            Position = position;
            _owner = owner;
            _distance = distanceToOwner();
            _direction = spinDirection;
            _velocity = Math.Max(owner.Velocity.Length() * 1.1f, 250f);

        }

        public override void Update(GameTime gameTime)
        {
            var line = new Line(_owner.Transform.Position, Position);
            if (line.Length > _distance)
            {
                _owner.Transform.Position = Position + (line.Start - line.End).Normalized() * _distance;
            }
            var sign = _direction == LookDirection.Right ? -1f : 1f;
            _owner.Velocity = line.Normal * sign * _velocity;
            
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Destroy()
        {
            base.Destroy();
            //Destroy all chain pieces as well

        }

        private float distanceToOwner()
        {
            return (Position - _owner.Transform.Position).Length();
        }
    }
}
