using UnityEngine;

namespace Common
{
    public interface ITransform
    {
        public Vector3 position { get; set; }
        public Quaternion rotation { get; set; }
        public Vector3 forward { get; }
        public Vector3 right { get; }
    }
}
