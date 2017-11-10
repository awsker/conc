namespace conc.game.entity.animation
{
    public interface IAnimation
    {
        AnimationType Type { get; set; }
        IAnimationFrame[] Frames { get; set; }
    }

    public class Animation : IAnimation
    {
        public AnimationType Type { get; set; }
        public IAnimationFrame[] Frames { get; set; }
    }
}