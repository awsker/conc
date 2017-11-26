using Microsoft.Xna.Framework;
using tile;

namespace conc.game.entity.movement
{
    public interface IMovementHandler
    {
        void HandleMovement(GameTime time, IMovingEntity entity, ILevel level);
    }
}
