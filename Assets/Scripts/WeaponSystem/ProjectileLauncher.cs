using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

namespace WeaponSystem
{
    public class ProjectileLauncher : Weapon
    {
        private IProjectileFactory projectileFactory;
        
        public ProjectileLauncher(ITransform barrel, int maximumAmmo, float bulletsPerSecond, IProjectileFactory projectileFactory):base(barrel, maximumAmmo, bulletsPerSecond)
        {
            this.projectileFactory = projectileFactory;
        }

        public override bool Fire()
        {
            if (base.Fire())
            {
                ITransform newProjectile = projectileFactory.GetProjectile(); // GameObject.Instantiate<Projectile>(projectile);
                newProjectile.position = Barrel.position; // newProjectile.transform.position = Barrel.position;
                newProjectile.rotation = Barrel.rotation; // newProjectile.transform.rotation = Barrel.rotation;
                return true;
            }
            return false;
        }
    }
}
