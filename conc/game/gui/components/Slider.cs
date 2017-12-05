namespace conc.game.gui.components
{
    public interface ISlider
    {
        float Min { get; }
        float Max { get; }
        float Current { get; }
    }

    public class Slider : ISlider
    {
        public Slider(int row, int col, float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Min { get; }
        public float Max { get; }
        public float Current { get; }
    }
}