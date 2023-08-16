using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    public interface IHomingProjectile
    {
        public Transform Target { get; set; }
    }
}
