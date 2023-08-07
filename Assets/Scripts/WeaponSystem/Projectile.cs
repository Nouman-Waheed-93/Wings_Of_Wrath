using UnityEngine;
namespace WeaponSystem
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float speed;

        private Rigidbody rigidbody;

        protected virtual void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        protected virtual void FixedUpdate()
        {
            rigidbody.velocity = transform.forward * speed;
        }
    }
}
