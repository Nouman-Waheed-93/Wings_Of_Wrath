using UnityEngine;
using Common;

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

        private ITimeProvider timeProvider;
        public ITimeProvider TimeProvider { get => timeProvider; }

        public Weapon(ITransform barrel, ITimeProvider timeProvider, int maximumAmmo, float bulletsPerSecond)
        {
            this.barrel = barrel;
            this.timeProvider = timeProvider;
            this.maximumAmmo = maximumAmmo;
            remainingAmmo = maximumAmmo;
            shotInterval = 1f / bulletsPerSecond;
            lastFireTime = -300f;
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
            lastFireTime = timeProvider.GetTime();
            return true;
        }

        public virtual void Reload()
        {
            remainingAmmo = maximumAmmo;
        }

        protected bool HasShotIntervalPassed()
        {
            return lastFireTime + ShotInterval <= timeProvider.GetTime();
        }

        protected bool HasAmmoRanOut()
        {
            return remainingAmmo <= 0;
        }
    }
}
