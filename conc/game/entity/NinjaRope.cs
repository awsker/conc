using System;
using conc.game.entity.baseclass;
using conc.game.entity.movement;
using conc.game.extensions;
using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using tile.math;

namespace conc.game.entity
{
    public class NinjaRope : Entity
    {
        private IMovingEntity _owner;
        private LookDirection _direction;

        private float _distance;
        private float _velocity;

        private bool _retracting;

        private Texture2D _ropeTex;

        public NinjaRope(Vector2 position, IMovingEntity owner, LookDirection spinDirection)
        {
            Position = position;

            _owner = owner;
            _distance = distanceToOwner();
            _direction = spinDirection;
            _velocity = Math.Max(owner.Velocity.Length() * 1.1f, 250f);

        }

        public override void LoadContent(ContentManager contentManager)
        {
            var cm = Scene.GameManager.Get<ColorManager>();
            _ropeTex = cm.Get(Color.Aquamarine);
        }

        public override void Update(GameTime gameTime)
        {
            if (!_retracting)
            {
                var line = new Line(_owner.Transform.Position, Position);
                if (line.Length > _distance)
                {
                    _owner.Transform.Position = Position + (line.Start - line.End).Normalized() * _distance;
                }
                var sign = _direction == LookDirection.Right ? -1f : 1f;
                _owner.Velocity = line.Normal * sign * _velocity;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawLine(_owner.Transform.Position, Position,);
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
