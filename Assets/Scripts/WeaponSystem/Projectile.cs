using Common;
using UnityEngine;

namespace WeaponSystem
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour, ITransform
    {
        [SerializeField]
        private float speed;

        private new Rigidbody rigidbody;
        private new Transform transform;

        public Vector3 position { get { return transform.position; } set { transform.position = value; } }
        public Quaternion rotation { get { return transform.rotation; } set { transform.rotation = value; } }
        public Vector3 forward { get => transform.forward; }

        protected virtual void Awake()
        {
            this.transform = base.transform;
            rigidbody = GetComponent<Rigidbody>();
        }

        protected virtual void FixedUpdate()
        {
            rigidbody.velocity = transform.forward * speed;
        }
    }
}
