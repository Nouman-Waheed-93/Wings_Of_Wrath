using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    public class GuidedProjectileLauncher : Weapon
    {

        private HomingProjectile projectile;

        private Transform target;
        public Transform Target { get => target; set => target = value; }

        public GuidedProjectileLauncher(Transform barrel, int maximumAmmo, float bulletsPerSecond, HomingProjectile projectilePrefab) : base(barrel, maximumAmmo, bulletsPerSecond)
        {
            projectile = projectilePrefab;
        }

        public override void Fire()
        {
            if (HasAmmoRanOut())
            {
                return;
            }

            if (!HasShotIntervalPassed())
            {
                return;
            }

            base.Fire();
            HomingProjectile newProjectile = GameObject.Instantiate<HomingProjectile>(projectile);
            newProjectile.Target = target;
        }
    }
}
