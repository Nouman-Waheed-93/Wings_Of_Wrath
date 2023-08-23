using UnityEngine;

namespace WeaponSystem
{
    public interface IHomingProjectile : ITransform
    {
        public Transform Target { get; set; }
    }
}
