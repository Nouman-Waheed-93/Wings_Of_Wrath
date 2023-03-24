using UnityEngine;
using Utilities;

namespace AircraftController
{
    public class FinalApproach : State
    {
        public FinalApproach(AircraftStateMachine stateMachine, AircraftController aircraftController) 
            : base(stateMachine, aircraftController) { }

        public override void Enter()
        {
            aircraftController.Throttle = 0.3f;
            //deploy landing gear
        }

        public override void Exit()
        {
        }

        public override void Update(float simulationDeltaTime)
        {
            float targetSpeed = 10;
            float brakePressure = (aircraftController.MovementHandler.CurrSpeed - targetSpeed) / 20;
            aircraftController.MovementHandler.SetBrake(brakePressure);
            if (IsFinalApproachDone())
                MoveToTouchDown();
            else if (IsAbortIntended())
                AbortLanding();
        }

        private bool IsAbortIntended()
        {
            Vector3 planePosition = Vector3.zero;
            Vector3 pointOnApproachLine = Vector3Extensions.FindNearestPointOnLine(aircraftController.AirStripToLandOn.InitialApproach.position,
                aircraftController.AirStripToLandOn.FinalApproach.position, planePosition);
            if (Vector3.Distance(planePosition, pointOnApproachLine) > 10)
                return true;
            return false;
        }

        private void AbortLanding()
        {
            stateMachine.ChangeState(aircraftController.StateInAir);
            //retract landing gear
        }

        private bool IsFinalApproachDone()
        {
            return (Vector3.Distance(aircraftController.MovementHandler.Transform.position,
                aircraftController.AirStripToLandOn.FinalApproach.position) < 1);
        }

        private void MoveToTouchDown()
        {
            stateMachine.ChangeState(aircraftController.StateTouchDown);
        }
    }
}
