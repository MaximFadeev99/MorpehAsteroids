namespace MorpehAsteroids.Signals
{
    public class PlayerHealthChangedSignal
    {
        public readonly int CurrentHealth;

        public PlayerHealthChangedSignal(int currentHealth)
        {
            CurrentHealth = currentHealth;
        }
    }
}