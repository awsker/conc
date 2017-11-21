using conc.game.entity.baseclass;
using conc.game.entity.movement;
using Microsoft.Xna.Framework;
using tile.math;

namespace conc.game.entity
{
    public class RopeProjectile : Entity, IMovingEntity
    {
        public override int Width => 5;
        public override int Height => 5;
        public Vector2 Velocity { get; set; }

        private IMovingEntity _owner;

        public RopeProjectile(IMovingEntity owner)
        {
            _owner = owner;
            CollisionSettings = new CollisionSettings(true, ActionOnCollision.None, 0f, 0f);
        }

        public void OnCollisionWithBackground(MovementHandler handler, Vector2 collision, Line line)
        {
            Destroy();
            //Scene.AddEntity(new NinjaRope(_owner));
        }

        public CollisionSettings CollisionSettings { get; }
    }
}
