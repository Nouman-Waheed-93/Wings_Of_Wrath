using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    public interface IProjectile
    {
        public Vector3 position { get; set; }
        public Quaternion rotation { get; set; }
    }
}
