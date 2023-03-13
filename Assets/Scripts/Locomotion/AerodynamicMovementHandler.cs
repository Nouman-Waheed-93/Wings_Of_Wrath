using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Locomotion
{
    public class AerodynamicMovementHandler : MovementHandler
    {
        AerodynamicMovementData aerodynamicMovementData;
        
        public AerodynamicMovementHandler(AerodynamicMovementData aerodynamicMovementData, Transform transform, Rigidbody rigidbody):base(aerodynamicMovementData, transform, rigidbody)
        {
            this.aerodynamicMovementData = aerodynamicMovementData;
        }

        protected override void HandleMovement(float simulationDeltaTime)
        {
            Vector3 currVelocityDirection = rigidbody.velocity.normalized;
            Vector3 targetVelocityDirection = transform.forward;
            Vector3 velocityDirection = Vector3.RotateTowards(currVelocityDirection, targetVelocityDirection, 
                Mathf.PI * aerodynamicMovementData.velocityDirectionChangeFactor, 1);
            HandleCurrSpeed(simulationDeltaTime);
            rigidbody.velocity = velocityDirection.normalized * currSpeed;
            rigidbody.angularVelocity = Vector3.up * currTurn * aerodynamicMovementData.maxTurnSpeed;
        }
    }
}
