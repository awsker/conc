namespace conc.game.entity.animation
{
    public interface IAnimationFrame
    {
        int X { get; set; }
        int Y { get; set; }
        int OffsetX { get; set; }
        int OffsetY { get; set; }
    }

    public class AnimationFrame : IAnimationFrame
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
    }
}