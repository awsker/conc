using System.Collections.Generic;

namespace conc.game.entity.animation
{
    public interface IAnimationGroup
    {
        string Name { get; set; }
        string SpriteSheet { get; set; }
        int FrameWidth { get; set; }
        int FrameHeight { get; set; }

        Dictionary<AnimationType, Animation> Animations { get; }
    }

    public class AnimationGroup : IAnimationGroup
    {
        public string Name { get; set; }
        public string SpriteSheet { get; set; }
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }

        public Dictionary<AnimationType, Animation> Animations { get; } = new Dictionary<AnimationType, Animation>();
    }
}
