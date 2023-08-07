using UnityEngine;

namespace WeaponSystem
{
    public abstract class Weapon
    {
        private int maximumAmmo;
        public int MaximumAmmo { get => maximumAmmo; }
        
        protected int remainingAmmo;

        protected float lastFireTime;

        private float shotInterval;
        public float ShotInterval { get => shotInterval; }

        private Transform barrel;
        protected Transform Barrel { get => barrel; }

        public Weapon(Transform barrel, int maximumAmmo, float bulletsPerSecond)
        {
            this.barrel = barrel;
            this.maximumAmmo = maximumAmmo;
            remainingAmmo = maximumAmmo;
            shotInterval = 1f / bulletsPerSecond;
        }

        public virtual void Fire()
        {
            lastFireTime = Time.time;
        }

        public virtual void Reload()
        {
            remainingAmmo = maximumAmmo;
        }

        protected bool HasShotIntervalPassed()
        {
            return lastFireTime + ShotInterval <= Time.time;
        }

        protected bool HasAmmoRanOut()
        {
            return remainingAmmo <= 0;
        }
    }
}
