using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace AircraftController
{
    public class TakeOff : State
    {
        private PIDController heightPIDController = new PIDController();
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
            float gearRetractHeight = 50;
            float inAirHeight = 100;
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
            float distance = Mathf.Abs(100 - aircraftController.MovementHandler.Transform.position.y);
            targetPosition += transformForward * 50;
            Vector3 relative = aircraftController.MovementHandler.Transform.InverseTransformPoint(targetPosition);
            float targetPitch = Mathf.Atan2(relative.y, relative.z);
            aircraftController.MovementHandler.SetPitch(-targetPitch);
        }
    }
}
