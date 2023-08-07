using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

namespace WeaponSystem
{
    public class ProjectileLauncher : Weapon
    {
        private Projectile projectile;
        
        public ProjectileLauncher(Transform barrel, int maximumAmmo, float bulletsPerSecond, Projectile projectilePrefab):base(barrel, maximumAmmo, bulletsPerSecond)
        {
            projectile = projectilePrefab;
        }

        public override void Fire()
        {
            if(HasAmmoRanOut())
            {
                return;
            }
            if (!HasShotIntervalPassed())
            {
                return;
            }

            base.Fire();
            Projectile newProjectile = GameObject.Instantiate<Projectile>(projectile);
            newProjectile.transform.position = Barrel.position;
            newProjectile.transform.rotation = Barrel.rotation;
        }
    }
}
