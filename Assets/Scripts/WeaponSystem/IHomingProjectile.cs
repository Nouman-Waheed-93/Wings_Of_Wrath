using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    public interface IHomingProjectile : ITransform
    {
        public Transform Target { get; set; }
    }
}
