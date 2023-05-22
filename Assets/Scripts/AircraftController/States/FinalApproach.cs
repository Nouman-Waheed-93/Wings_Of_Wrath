using UnityEngine;

namespace AircraftController
{
    public class FinalApproach : State
    {
        public FinalApproach(AircraftStateMachine stateMachine, AircraftController aircraftController) 
            : base(stateMachine, aircraftController) { }

        public override void Enter()
        {
            //deploy landing gear
        }

        public override void Exit()
        {
        }

        public override void Update(float simulationDeltaTime)
        {
            aircraftController.SeekSpeed(GlobalAircraftControllerSettings.finalApproachSpeed);
            LowerAltitude();
            aircraftController.MovementHandler.Turn(aircraftController.TurnInput);
            if (IsFinalApproachDone())
            {
                MoveToTouchDown();
                Debug.Log("Touching down");
            }
            else if (IsAbortIntended())
            {
                AbortLanding();
                Debug.Log("Aborted Landing");
            }
        }

        private bool IsAbortIntended()
        {
            return aircraftController.HasDeviatedFromLine(aircraftController.AirStripToLandOn.InitialApproach.position, 
                aircraftController.AirStripToLandOn.FinalApproach.position, GlobalAircraftControllerSettings.finalApproachAcceptableDeviation);
        }

        private void AbortLanding()
        {
            stateMachine.ChangeState(aircraftController.StateInAir);
            aircraftController.AirStripToLandOn = null;
            //retract landing gear
        }

        private bool IsFinalApproachDone()
        {
            return (Vector3.Distance(aircraftController.Transform.position,
                aircraftController.AirStripToLandOn.FinalApproach.position) < GlobalAircraftControllerSettings.wayPointReachedDistance);
        }

        private void MoveToTouchDown()
        {
            stateMachine.ChangeState(aircraftController.StateTouchDown);
        }
        
        private void LowerAltitude()
        {
            aircraftController.CalculateAndSetPitch(aircraftController.AirStripToLandOn.FinalApproach.position);
        }
    }
}
