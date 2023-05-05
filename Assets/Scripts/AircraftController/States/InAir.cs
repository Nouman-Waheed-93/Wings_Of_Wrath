using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AircraftController
{
    public class InAir : State
    {
        public InAir(AircraftStateMachine stateMachine, AircraftController aircraftController) : base(stateMachine, aircraftController)
        {
            
        }

        public override void Enter()
        {
            aircraftController.MovementHandler.SetThrottle(1);
            aircraftController.MovementHandler.SetBrake(0);
        }

        public override void Exit()
        {

        }

        public override void Update(float simulationDeltaTime)
        {
            //Can Fire Weapons
            //Keep Height
            if (IsInitialApproachDone())
            {
                MoveToFinalApproach();
            }
            KeepAltitude();
            aircraftController.MovementHandler.Turn(aircraftController.Turn);
        }

        private bool IsInitialApproachDone()
        {
            return aircraftController.AirStripToLandOn != null;
        }
        private void MoveToFinalApproach()
        {
            stateMachine.ChangeState(aircraftController.StateFinalApproach);
        }

        private void KeepAltitude()
        {
            Vector3 targetPosition = aircraftController.MovementHandler.Transform.position;
            targetPosition.y = 100;
            Vector3 transformForward = aircraftController.MovementHandler.Transform.forward;
            transformForward.y = 0;
            transformForward.Normalize();
            float distance = Mathf.Abs(100 - aircraftController.MovementHandler.Transform.position.y);
            targetPosition += transformForward * 100;
            Vector3 relative = aircraftController.MovementHandler.Transform.InverseTransformPoint(targetPosition);
            float targetPitch = Mathf.Atan2(relative.y, relative.z);
            aircraftController.MovementHandler.SetPitch(-targetPitch);
        }
    }
}
