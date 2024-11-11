namespace MorpehAsteroids.Signals
{
    public class MouseScrolledSignal
    {
        public readonly bool HasScrolledUp;

        public MouseScrolledSignal(bool hasScrolledUp)
        {
            HasScrolledUp = hasScrolledUp;
        }
    }
}