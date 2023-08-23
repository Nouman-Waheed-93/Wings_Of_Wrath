using UnityEngine;

namespace WeaponSystem
{
    public abstract class MonoBehaviourWeapon : MonoBehaviour, ITransform
    {
        [SerializeField]
        protected int maxAmmo;
        [SerializeField]
        protected float bulletsPerSecond;
        [SerializeField]
        protected GameObject barrelGO;

        protected Weapon weapon;

        private new Transform transform;
        public Vector3 position { get => transform.position; set => transform.position = value; }
        public Quaternion rotation { get => transform.rotation; set => transform.rotation = value; }
        public Vector3 forward => transform.forward;

        protected virtual void Awake()
        {
            this.transform = base.transform;
        }
    }
}
