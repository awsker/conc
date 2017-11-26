namespace conc.game.gui.components
{
    public interface ISliderComponent
    {
        float Min { get; }
        float Max { get; }
        float Current { get; }
    }

    public class SliderComponent : ISliderComponent
    {
        public SliderComponent(int row, int col, float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Min { get; }
        public float Max { get; }
        public float Current { get; }
    }
}