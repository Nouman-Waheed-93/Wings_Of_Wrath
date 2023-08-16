using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    public interface IProjectileFactory
    {
        public ITransform GetProjectile();
        public IHomingProjectile GetHomingProjectile();
    }
}
