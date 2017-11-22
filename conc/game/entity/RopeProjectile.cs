using conc.game.entity.baseclass;
using conc.game.entity.movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using tile.math;

namespace conc.game.entity
{
    public class RopeProjectile : Entity, IMovingEntity
    {
        public override int Width => 5;
        public override int Height => 5;

        public Vector2 Velocity { get; set; }

        private Texture2D _texture;

        private IMovingEntity _owner;
        private LookDirection _direction;

        private IEntity _ropeEntity;

        public RopeProjectile(IMovingEntity owner, LookDirection direction)
        {
            _owner = owner;
            CollisionSettings = new CollisionSettings(true, ActionOnCollision.None, 0f, 0f);
            _texture = owner.Scene.GameManager.Get<ContentManager>().Load<Texture2D>("trash/ropeprojectile");
            _direction = direction;
            Position = owner.Transform.Position;
        }

        public void OnCollisionWithBackground(MovementHandler handler, Vector2 collision, Line line)
        {
            Velocity = Vector2.Zero;
            CollisionSettings.CheckCollisionsWithBackground = false;
            if (_ropeEntity == null)
            {
                _ropeEntity = new NinjaRope(Position, _owner, _direction);
                Scene.AddEntity(_ropeEntity);
            }
        }

        public CollisionSettings CollisionSettings { get; }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(_texture, Position - new Vector2(3f, 3f), Color.White);
        }

        public override void Destroy()
        {
            base.Destroy();
            _ropeEntity?.Destroy();
        }
    }
}
