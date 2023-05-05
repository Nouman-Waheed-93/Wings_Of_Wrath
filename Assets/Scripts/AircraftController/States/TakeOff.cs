using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace AircraftController
{
    public class TakeOff : State
    {
        public TakeOff(AircraftStateMachine stateMachine, AircraftController aircraftController) : base(stateMachine, aircraftController)
        {
        }

        public override void Enter()
        {
            //Play Airborne audio
        }

        public override void Exit()
        {

        }

        public override void Update(float simulationDeltaTime)
        {
            CalculateAndSetPitch();
            float gearRetractHeight = 20;
            float inAirHeight = 30;
            if(aircraftController.MovementHandler.Height > gearRetractHeight)
            {
                //retract gear
            }
            if(aircraftController.MovementHandler.Height >= inAirHeight)
            {
                stateMachine.ChangeState(aircraftController.StateInAir);
            }
        }

        private void CalculateAndSetPitch()
        {
            Vector3 targetPosition = aircraftController.MovementHandler.Transform.position;
            targetPosition.y = 100;
            Vector3 transformForward = aircraftController.MovementHandler.Transform.forward;
            transformForward.y = 0;
            transformForward.Normalize();
            targetPosition += transformForward * 300f;
            Vector3 relative = aircraftController.MovementHandler.Transform.InverseTransformPoint(targetPosition);
            float targetPitch = Mathf.Atan2(relative.y, relative.z);
            aircraftController.MovementHandler.SetPitch(-targetPitch);
        }
    }
}
