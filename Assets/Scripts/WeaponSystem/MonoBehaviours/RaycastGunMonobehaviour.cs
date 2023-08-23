using UnityEngine;
using Common;

namespace WeaponSystem
{
    public class RaycastGunMonobehaviour : MonoBehaviourWeapon
    {
        [SerializeField]
        private float range;

        private void Awake()
        {
            base.Awake();
            weapon = new GunRaycastBased(this, new GameTimeProvider(), maxAmmo, bulletsPerSecond, range);
        }
    }
}
