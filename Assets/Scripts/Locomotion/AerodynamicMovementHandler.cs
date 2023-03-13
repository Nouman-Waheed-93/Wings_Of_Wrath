using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Locomotion
{
    public class AerodynamicMovementHandler : MovementHandler
    {
        private AerodynamicMovementData aerodynamicMovementData;
        private float currPitch;

        public AerodynamicMovementHandler(AerodynamicMovementData aerodynamicMovementData, Transform transform, Rigidbody rigidbody):base(aerodynamicMovementData, transform, rigidbody)
        {
            this.aerodynamicMovementData = aerodynamicMovementData;
        }

        public void SetPitch(float pitch)
        {
            currPitch = Mathf.Clamp(pitch, -1, 1);
        }

        protected override void HandleMovement(float simulationDeltaTime)
        {
            Vector3 currVelocityDirection = rigidbody.velocity.normalized;

            if(currVelocityDirection.sqrMagnitude == 0) 
                currVelocityDirection = transform.forward;
            
            Vector3 targetVelocityDirection = transform.forward;
            float speedFactorOnTurn = Mathf.Clamp01(currSpeed / aerodynamicMovementData.turnMaximizationSpeed);
            Vector3 velocityDirection = Vector3.RotateTowards(currVelocityDirection, targetVelocityDirection, 
                Mathf.PI * aerodynamicMovementData.velocityDirectionChangeFactor * speedFactorOnTurn, 0);
            HandleCurrSpeed(simulationDeltaTime);
            rigidbody.velocity = velocityDirection * currSpeed;
            Vector3 pitchVelocity = transform.right * currPitch * aerodynamicMovementData.maxTurn *speedFactorOnTurn;
            Vector3 turnVelocity = Vector3.up * currTurn * aerodynamicMovementData.maxTurn * speedFactorOnTurn;
            rigidbody.angularVelocity = pitchVelocity + turnVelocity;
        }
    }
}
