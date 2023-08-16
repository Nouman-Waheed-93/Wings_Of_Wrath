using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    public class GuidedProjectileLauncher : Weapon
    {

        private IProjectileFactory projectileFactory;

        private Transform target;
        public Transform Target { get => target; set => target = value; }

        public GuidedProjectileLauncher(Transform barrel, int maximumAmmo, float bulletsPerSecond, IProjectileFactory projectileFactory) : base(barrel, maximumAmmo, bulletsPerSecond)
        {
            this.projectileFactory = projectileFactory;
        }

        public override bool Fire()
        {
            if (base.Fire())
            {
                IHomingProjectile newProjectile = projectileFactory.GetHomingProjectile(); // GameObject.Instantiate<HomingProjectile>(projectile);
                newProjectile.Target = target;
                return true;
            }
            return false;
        }
    }
}
