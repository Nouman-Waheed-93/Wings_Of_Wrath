using UnityEngine;

namespace AircraftController
{
    public class TouchDown : State
    {
        public TouchDown(AircraftStateMachine stateMachine, AircraftController aircraftController) :
            base(stateMachine, aircraftController)
        {
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void Update(float simulationDeltaTime)
        {
            LowerAltitude();
            aircraftController.SeekSpeed(GlobalAircraftControllerSettings.touchDownSpeed);
            aircraftController.MovementHandler.Turn(aircraftController.TurnInput);
            if (IsAbortIntended())
            {
                Debug.Log("Aborting");
                if (CanAbort())
                {
                    Debug.Log("Do Abort");
                    DoAbort();
                }
                else
                {
                    Debug.Log("Do Crash");
                    DoCrash();
                }
            }
            else if (IsTouchDownDone())
            {
                Debug.Log("Done touch down");
                DoLand();
            }
        }

        private bool IsAbortIntended()
        {
            return aircraftController.HasDeviatedFromLine(aircraftController.AirStripToLandOn.FinalApproach.position,
                aircraftController.AirStripToLandOn.TouchDownPoint.position, GlobalAircraftControllerSettings.touchDownAcceptableDeviation);
        }

        private bool CanAbort()
        {
            return true;
        }

        private void DoAbort()
        {
            stateMachine.ChangeState(aircraftController.StateInAir);
            aircraftController.AirStripToLandOn = null;
        }

        private void DoCrash()
        {
        }

        private bool IsTouchDownDone()
        {
            return (Vector3.Distance(aircraftController.MovementHandler.Transform.position,
                aircraftController.AirStripToLandOn.TouchDownPoint.position) < GlobalAircraftControllerSettings.wayPointReachedDistance);
        }

        private void DoLand()
        {
            stateMachine.ChangeState(aircraftController.StateLanded);
        }

        private void LowerAltitude()
        {
            aircraftController.CalculateAndSetPitch(aircraftController.AirStripToLandOn.TouchDownPoint.position);
        }
    }
}
