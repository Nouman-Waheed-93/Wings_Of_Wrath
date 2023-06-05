namespace HealthSystem
{
    public class DamageablePartController
    {
        private Health health;
        private float damageMultiplier;

        public DamageablePartController(Health health, float damageMultiplier)
        {
            this.health = health;
            this.damageMultiplier = damageMultiplier;
        }

        public void Damage(float amount)
        {
            health.ReduceHealth(amount * damageMultiplier);
        }
    }
}
