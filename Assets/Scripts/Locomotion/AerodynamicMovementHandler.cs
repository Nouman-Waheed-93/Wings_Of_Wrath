using UnityEngine;
using Utilities;

namespace Locomotion
{
    public class AerodynamicMovementHandler : MovementHandler, IPitchYaw
    {
        private AerodynamicMovementData aerodynamicMovementData;
        public AerodynamicMovementData AerodynamicMovementData { get => aerodynamicMovementData; }

        private TargetValueSeeker pitchSeeker;
        private TargetValueSeeker turnSeeker;

        public float TurnFactor 
        { 
            get
            {
                return turnSeeker.CurrValue;
            } 
        }

        public float PitchFactor 
        {
            get 
            {
                return transform.forward.y;
            }
        }

        public float Height { get { return transform.position.y; } }

        public AerodynamicMovementHandler(AerodynamicMovementData aerodynamicMovementData, Transform transform, Rigidbody rigidbody):base(aerodynamicMovementData, transform, rigidbody)
        {
            this.aerodynamicMovementData = aerodynamicMovementData;
            pitchSeeker = new TargetValueSeeker(1);
            turnSeeker = new TargetValueSeeker(1);
        }

        public void SetPitch(float pitch)
        {
            pitchSeeker.Target = Mathf.Clamp(pitch, -1, 1);
        }

        protected override void HandleMovement(float simulationDeltaTime)
        {
            turnSeeker.Target = currTurn;
            turnSeeker.Seek(simulationDeltaTime);
            pitchSeeker.Seek(simulationDeltaTime);
            HandleCurrSpeed(simulationDeltaTime);

            rigidbody.velocity = transform.forward * currSpeed;

            Vector3 pitchVelocity = transform.right * pitchSeeker.CurrValue * aerodynamicMovementData.maxPitch;
            Vector3 turnVelocity = Vector3.up * turnSeeker.CurrValue * aerodynamicMovementData.maxTurn;
            rigidbody.angularVelocity = pitchVelocity + turnVelocity;
        }
    }
}
