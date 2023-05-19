using UnityEngine;

namespace Locomotion
{
    public class MovementHandler : IAcceleratable, ITurnable
    {
        [SerializeField]
        private MovementData movementData;

        private float targetSpeed;
        private float currAcceleration;
        private float currDeceleration;
        private float currBrake;

        protected float currSpeed;
        public float CurrSpeed { get => currSpeed; }

        protected float currTurn;

        protected Transform transform;
        public Transform Transform { get => transform; }

        protected Rigidbody rigidbody;

        public MovementHandler(MovementData movementData, Transform transform, Rigidbody rigidbody)
        {
            this.movementData = movementData;
            this.transform = transform;
            this.rigidbody = rigidbody;
        }

        public void Update(float simulationDeltaTime)
        {
            HandleMovement(simulationDeltaTime);
        }

        /// <summary>
        /// throttle can be between 0(min throttle) and 1(max throttle)
        /// </summary>
        /// <param name="throttle"></param>
        public void SetThrottle(float throttle)
        {
            this.targetSpeed = Mathf.Lerp(0, movementData.maxSpeed, throttle);
            this.currAcceleration = Mathf.Lerp(0, movementData.maxAcceleration, throttle);
            this.currDeceleration = Mathf.Lerp(movementData.maxDeceleration, 0, throttle);
        }

        public void SetBrake(float brakePressure)
        {
            this.currBrake = Mathf.Lerp(0, movementData.maxBrake, brakePressure);
        }

        public void Turn(float direction)
        {
            direction = Mathf.Clamp(direction, -1, 1);
            this.currTurn = direction;
        }

        protected virtual void HandleMovement(float simulationDeltaTime)
        {
            HandleCurrSpeed(simulationDeltaTime);
            rigidbody.velocity = transform.forward * currSpeed;
            rigidbody.angularVelocity = Vector3.up * currTurn * movementData.maxTurn;
        }
    
        protected void HandleCurrSpeed(float simulationDeltaTime)
        {
            if (currSpeed < targetSpeed)
            {
                currSpeed += currAcceleration * simulationDeltaTime;
                if(currSpeed > targetSpeed)
                {
                    currSpeed = targetSpeed;
                }
            }
            else if(currSpeed > targetSpeed)
            {
                currSpeed -= currDeceleration * simulationDeltaTime;
                if(currSpeed < 0)
                {
                    currSpeed = 0;
                }    
            }

            currSpeed -= currBrake * simulationDeltaTime;
            currSpeed = Mathf.Clamp(currSpeed, 0, movementData.maxSpeed);
        }
    }
}
