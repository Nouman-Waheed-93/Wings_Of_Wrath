using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HealthSystem
{
    public class DamageablePart : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private GameObject HealthObject;

        [SerializeField]
        private float damageMultiplier;

        private Health health;

        private void Awake()
        {
            health = HealthObject.GetComponent<IHealthProvider>().health;
        }

        public void Damage(float amount)
        {
            health.ReduceHealth(amount * damageMultiplier);
        }
    }
}
