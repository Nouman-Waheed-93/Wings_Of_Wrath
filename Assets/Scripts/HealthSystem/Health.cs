using System;

namespace HealthSystem
{
    public class Health
    {
        private float maxHealth;

        private float currHealth;
        public float CurrHealth { get => currHealth; }

        public event Action onHealthDepleted;
        public event Action onHealthReplenished;

        public event Action onHealthReduced;
        public event Action onHealthGained;
        public Health(float maxHealth)
        {
            this.maxHealth = maxHealth;
            this.currHealth = maxHealth;
        }

        public Health(float maxHealth, float currHealth)
        {
            this.maxHealth = maxHealth;
            this.currHealth = currHealth;
        }

        public void ReduceHealth(float reduceAmount)
        {
            if (reduceAmount <= 0)
                return;

            currHealth -= reduceAmount;
            onHealthReduced?.Invoke();

            if (currHealth <= 0)
            {
                currHealth = 0;
                onHealthDepleted?.Invoke();
            }
        }

        public void ReplenishHealth(float replenishAmount)
        {
            if (replenishAmount <= 0)
                return;

            currHealth += replenishAmount;
            onHealthGained?.Invoke();

            if(currHealth >= maxHealth)
            {
                currHealth = maxHealth;
                onHealthReplenished?.Invoke();
            }
        }
    }
}
