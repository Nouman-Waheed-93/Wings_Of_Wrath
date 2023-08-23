using UnityEngine;
using HealthSystem;
using Common;

namespace WeaponSystem
{
    public class GunRaycastBased : Weapon
    {
        private float range;
        private float damageAmount;

        public GunRaycastBased(ITransform barrel, ITimeProvider timeProvider, int maximumAmmo, float bulletsPerSecond, float range) : base(barrel, timeProvider, maximumAmmo, bulletsPerSecond)
        {
            this.range = range;
        }

        //Should be called in FixedUpdate
        public override bool Fire()
        {
            if (base.Fire())
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(Barrel.position, Barrel.forward, out hitInfo, range))
                {
                    IDamageable damageable;
                    if (hitInfo.collider.TryGetComponent<IDamageable>(out damageable))
                    {
                        damageable.Damage(damageAmount);
                    }
                }
                return true;
            }
            return false;
        }

    }
}
