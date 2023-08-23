using Common;
using UnityEngine;

namespace WeaponSystem
{
    public class ProjectileFactory : MonoBehaviour, IProjectileFactory
    {
        [SerializeField]
        private GameObject simpleProjectilePrefab;
        [SerializeField]
        private GameObject homingProjectilePrefab;

        public IHomingProjectile GetHomingProjectile()
        {
            return Instantiate(homingProjectilePrefab).GetComponent<IHomingProjectile>();
        }

        public ITransform GetProjectile()
        {
            return Instantiate(simpleProjectilePrefab).GetComponent<ITransform>();
        }
    }
}
