using conc.game.entity.animation;
using conc.game.entity.baseclass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace conc.game.entity
{
    public interface IPlayer : IAnimatedEntity
    {
    }

    public class Player : AnimatedEntity, IPlayer
    {
        public Player(Vector2 position, IAnimator animator) : base(position, animator)
        {
        }
    }
}