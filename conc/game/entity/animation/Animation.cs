using System.Collections.Generic;

namespace conc.game.entity.animation
{
    public interface IAnimation
    {
        AnimationType Type { get; set; }
        IList<IAnimationFrame> Frames { get; }
    }

    public class Animation : IAnimation
    {
        public AnimationType Type { get; set; }
        public IList<IAnimationFrame> Frames { get; } = new List<IAnimationFrame>();
    }
}