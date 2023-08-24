using UnityEngine;
using Utilities;

namespace Locomotion
{
    public class AircraftMovementHandler : MovementHandler, IPitchYaw
    {
        private AircraftMovementData aerodynamicMovementData;
        public AircraftMovementData AerodynamicMovementData { get => aerodynamicMovementData; }

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

        public float Altitude { get { return transform.position.y; } }

        public AircraftMovementHandler(AircraftMovementData aerodynamicMovementData, Transform transform, Rigidbody rigidbody):base(aerodynamicMovementData, transform, rigidbody)
        {
            this.aerodynamicMovementData = aerodynamicMovementData;
            pitchSeeker = new TargetValueSeeker(aerodynamicMovementData.pitchSpeed);
            turnSeeker = new TargetValueSeeker(aerodynamicMovementData.rollSpeed);
        }

        public void Initialize(float startSpeed, float startAltitude)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = startAltitude;
            transform.position = newPosition;

            currSpeed = startSpeed;
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
            CorrectYRotation();
        }

        private void CorrectYRotation()
        {
            transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        }
    }
}
