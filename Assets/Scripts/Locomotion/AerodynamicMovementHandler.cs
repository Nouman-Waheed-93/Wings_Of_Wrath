using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Locomotion
{
    public class AerodynamicMovementHandler : MovementHandler, ITurnFactor
    {
        private AerodynamicMovementData aerodynamicMovementData;
        private TargetValueSeeker pitchSeeker;
        private TargetValueSeeker turnSeeker;
        private float currPitch;

        public float TurnFactor { 
            get {
               return turnSeeker.CurrValue * Mathf.Clamp01(currSpeed / aerodynamicMovementData.turnMaximizationSpeed);
            } 
        }

        public AerodynamicMovementHandler(AerodynamicMovementData aerodynamicMovementData, Transform transform, Rigidbody rigidbody):base(aerodynamicMovementData, transform, rigidbody)
        {
            this.aerodynamicMovementData = aerodynamicMovementData;
            pitchSeeker = new TargetValueSeeker(1);
            turnSeeker = new TargetValueSeeker(1);
        }

        public void SetPitch(float pitch)
        {
            currPitch = Mathf.Clamp(pitch, -1, 1);
        }

        protected override void HandleMovement(float simulationDeltaTime)
        {
            float speedDependentTurnFactor = Mathf.Clamp01(currSpeed / aerodynamicMovementData.turnMaximizationSpeed);
            turnSeeker.Target = currTurn;
            turnSeeker.Seek(simulationDeltaTime);
            HandleCurrSpeed(simulationDeltaTime);

            Vector3 currVelocityDirection = rigidbody.velocity.normalized;

            if(currVelocityDirection.sqrMagnitude == 0) 
                currVelocityDirection = transform.forward;

            Vector3 targetVelocityDirection = transform.forward;
            Vector3 velocityDirection = Vector3.RotateTowards(currVelocityDirection, targetVelocityDirection, 
                Mathf.PI * aerodynamicMovementData.velocityDirectionChangeFactor * speedDependentTurnFactor, 0);
            rigidbody.velocity = velocityDirection * currSpeed;
            Vector3 pitchVelocity = transform.right * currPitch * aerodynamicMovementData.maxTurn * speedDependentTurnFactor;
            Vector3 turnVelocity = Vector3.up * turnSeeker.CurrValue * aerodynamicMovementData.maxTurn * speedDependentTurnFactor;
            rigidbody.angularVelocity = pitchVelocity + turnVelocity;
        }

    }
}
