using UnityEngine;

namespace WeaponSystem
{
    public abstract class Weapon
    {
        private int maximumAmmo;
        public int MaximumAmmo { get => maximumAmmo; }
        
        protected int remainingAmmo;

        public int RemainingAmmo { get => remainingAmmo; }

        protected float lastFireTime;

        private float shotInterval;
        public float ShotInterval { get => shotInterval; }

        private ITransform barrel;
        protected ITransform Barrel { get => barrel; }

        public Weapon(int maximumAmmo, float bulletsPerSecond)
        {
            this.maximumAmmo = maximumAmmo;
            remainingAmmo = maximumAmmo;
            shotInterval = 1f / bulletsPerSecond;
            lastFireTime = -300f;
        }

        public Weapon(ITransform barrel, int maximumAmmo, float bulletsPerSecond) : this(maximumAmmo, bulletsPerSecond)
        {
            this.barrel = barrel;
        }

        public virtual bool Fire()
        {
            if (HasAmmoRanOut())
            {
                return false;
            }
            if (!HasShotIntervalPassed())
            {
                return false;
            }
            remainingAmmo--;
            lastFireTime = Time.time;
            return true;
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
