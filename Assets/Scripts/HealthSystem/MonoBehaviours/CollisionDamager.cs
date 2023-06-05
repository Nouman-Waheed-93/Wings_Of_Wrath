using UnityEngine;

namespace HealthSystem
{
    public class CollisionDamager : MonoBehaviour
    {
        [SerializeField]
        private float damageAmount;

        private HitDamager hitDamager;

        private void Awake()
        {
            hitDamager = new HitDamager(damageAmount);
            Health health = new Health(10, 10);
        }

        private void OnCollisionEnter(Collision collision)
        {
            IDamageable damageable;
            if(collision.collider.TryGetComponent(out damageable))
            {
                hitDamager.Hit(damageable);
            }
        }
    }
}
