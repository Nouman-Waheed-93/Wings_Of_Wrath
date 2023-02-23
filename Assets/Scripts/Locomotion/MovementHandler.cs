using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Locomotion
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovementHandler : MonoBehaviour, IAcceleratable, ITurnable
    {
        private float throttle;
        private float turn;

        private Rigidbody rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            rigidbody.velocity = transform.forward * throttle;
            rigidbody.angularVelocity = Vector3.up * turn;
        }

        public void Accelerate(float throttle)
        {
            this.throttle = throttle;
        }

        public void Turn(float direction)
        {
            this.turn = direction;
        }
    }
}
