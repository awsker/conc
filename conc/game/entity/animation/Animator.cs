using System.Collections.Generic;

namespace conc.game.entity.animation
{
    public interface IAnimator
    {
        string Name { get; }
        string SpriteSheet { get; }
        int FrameWidth { get; }
        int FrameHeight { get; }

        Dictionary<AnimationType, IAnimation> Animations { get; }
    }

    public class Animator : IAnimator
    {
        public string Name { get; set; }
        public string SpriteSheet { get; set; }
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }

        public Dictionary<AnimationType, IAnimation> Animations { get; } = new Dictionary<AnimationType, IAnimation>();
    }
}
