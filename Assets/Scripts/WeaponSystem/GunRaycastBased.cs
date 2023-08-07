using UnityEngine;
using HealthSystem;

namespace WeaponSystem
{
    public class GunRaycastBased : Weapon
    {
        private float range;
        private float damageAmount;
        public GunRaycastBased(Transform barrel, int maximumAmmo, float bulletsPerSecond, float range) : base(barrel, maximumAmmo, bulletsPerSecond)
        {
            this.range = range;
        }

        //Should be called in FixedUpdate
        public override void Fire()
        {
            if(!HasShotIntervalPassed())
            {
                return;
            }

            if (HasAmmoRanOut())
            {
                return;
            }

            lastFireTime = Time.fixedTime;
            remainingAmmo--;

            RaycastHit hitInfo;
            if(Physics.Raycast(Barrel.position, Barrel.forward, out hitInfo, range))
            {
                IDamageable damageable;
                if(hitInfo.collider.TryGetComponent<IDamageable>(out damageable))
                {
                    damageable.Damage(damageAmount);
                }
            }
        }

    }
}
