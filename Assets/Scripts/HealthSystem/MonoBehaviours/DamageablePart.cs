using UnityEngine;

namespace HealthSystem
{
    public class DamageablePart : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private GameObject HealthObject;

        [SerializeField]
        private float damageMultiplier;

        private DamageablePartController controller;

        private void Awake()
        {
            Health health = HealthObject.GetComponent<IHealthProvider>().health;
            controller = new DamageablePartController(health, damageMultiplier);
        }

        public void Damage(float amount)
        {
            controller.Damage(amount);
        }
    }
}
