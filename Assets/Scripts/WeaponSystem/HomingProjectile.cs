using UnityEngine;

namespace WeaponSystem
{
    public class HomingProjectile : Projectile, IHomingProjectile
    {
        private Transform target;
        public Transform Target { get => target; set => target = value; }

        [SerializeField]
        private float turningSpeed;

        private new void Awake()
        {
            base.Awake();
        }

        private new void FixedUpdate()
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turningSpeed * Time.fixedDeltaTime);
            base.FixedUpdate();
        }
    }
}
