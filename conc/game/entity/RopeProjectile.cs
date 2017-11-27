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
    public class RopeProjectile : ImageEntity, IMovingEntity
    {
        public Vector2 Velocity { get; set; }

        private const float Speed = 800f;
        private const float MaxLength = 500f;
        
        private IMovingEntity _owner;
        private LookDirection _direction;

        private NinjaRope _ropeEntity;

        private bool _retracting;
        private Texture2D _ropeTex;

        private float _gravity;

        public RopeProjectile(IMovingEntity owner, LookDirection direction, float gravity):base(1, 1, new Point(0, 0), new Point(3, 3),  "trash/ropeprojectile")
        {
            _owner = owner;
            CollisionSettings = new CollisionSettings(true, ActionOnCollision.PushOut, 0f, 0f);

            _direction = direction;
            Position = owner.Transform.Position;
            Velocity = new Vector2(Speed * (direction == LookDirection.Left ? -1f : 1f), -Speed);
            _gravity = gravity;
        }

        public void OnCollisionWithBackground(IMovementHandler handler, Vector2 collision, Line line)
        {
            Velocity = Vector2.Zero;
            CollisionSettings.CheckCollisionsWithBackground = false;
            if (_ropeEntity == null && !_retracting)
            {
                _ropeEntity = new NinjaRope(Position, _owner, _direction, _gravity);
                Scene.AddEntity(_ropeEntity);
            }
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
            var cm = Scene.GameManager.Get<ColorManager>();
            _ropeTex = cm.Get(Color.Aquamarine);
        }

        public CollisionSettings CollisionSettings { get; }

        public override void Update(GameTime gameTime)
        {
            if (Length > MaxLength && !_retracting && _ropeEntity == null)
            {
                Retract();
            }
            if (_retracting)
            {
                if ((Position - _owner.Transform.Position).Length() < 30f)
                    Destroy();
                else
                    Velocity = (_owner.Transform.Position - Position).Normalized() * Speed * 1.5f;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawLine(_ropeTex, _owner.Transform.Position, Position, Color.White, 1);
            base.Draw(spriteBatch);
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

        private float Length => (Position - _owner.Transform.Position).Length();
    }
}
