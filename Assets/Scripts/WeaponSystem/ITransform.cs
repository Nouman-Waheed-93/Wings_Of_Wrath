using UnityEngine;

namespace WeaponSystem
{
    public interface ITransform
    {
        public Vector3 position { get; set; }
        public Quaternion rotation { get; set; }
        public Vector3 forward { get; }
    }
}
