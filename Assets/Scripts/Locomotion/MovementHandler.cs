using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Locomotion
{
    public class MovementHandler : IAcceleratable, ITurnable
    {
        [SerializeField]
        private MovementData movementData;

        private float throttle;
        private float turn;

        private Transform transform;
        private Rigidbody rigidbody;

        public MovementHandler(MovementData movementData, Transform transform, Rigidbody rigidbody)
        {
            this.movementData = movementData;
            this.transform = transform;
            this.rigidbody = rigidbody;
        }

        private void Update(float simulationDeltaTime)
        {
            rigidbody.velocity = transform.forward * throttle;
            rigidbody.angularVelocity = Vector3.up * turn * movementData.turnSpeed;
        }

        public void Accelerate(float throttle)
        {
            throttle = Mathf.Clamp(throttle, 0, 1);
            this.throttle = Mathf.Lerp(movementData.minThrottle, movementData.maxThrottle, throttle);
        }

        public void Turn(float direction)
        {
            direction = Mathf.Clamp(direction, -1, 1);
            this.turn = direction;
        }
    }
}
