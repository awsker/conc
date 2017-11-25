using System;
using Microsoft.Xna.Framework;
using tile;

namespace conc.game.entity.movement
{
    public class IntMovementHandler : IMovementHandler
    {
        public void HandleMovement(GameTime time, IMovingEntity entity, ILevel level)
        {
            moveStepwise(entity, Math.Sign(entity.Velocity.X), 0, level);
            moveStepwise(entity, 0, Math.Sign(entity.Velocity.Y), level);
        }

        private void moveStepwise(IMovingEntity entity, int dx, int dy, ILevel level)
        {
            
        }
    }
}
