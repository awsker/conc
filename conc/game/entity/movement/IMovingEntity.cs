using conc.game.entity.baseclass;
using Microsoft.Xna.Framework;
using tile.math;

namespace conc.game.entity.movement
{
    public interface IMovingEntity : IEntity
    {
        Vector2 Velocity { get; set; }
        void OnCollisionWithBackground(MovementHandler handler, Vector2 collision, Line line);
        CollisionSettings CollisionSettings { get; }
    }
}
