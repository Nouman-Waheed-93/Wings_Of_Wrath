namespace HealthSystem
{
    public class HitDamager
    {
        private float damageAmount;
        public HitDamager(float damageAmount)
        {
            this.damageAmount = damageAmount;
        }

        public void Hit(IDamageable damageable)
        {
            damageable.Damage(damageAmount);
        }
    }
}
