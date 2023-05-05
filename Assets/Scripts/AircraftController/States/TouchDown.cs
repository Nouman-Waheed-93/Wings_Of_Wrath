using UnityEngine;
using Utilities;

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
            aircraftController.MovementHandler.SetThrottle(0.2f);
            aircraftController.MovementHandler.SetBrake(0.3f);
        }

        public override void Exit()
        {
        }

        public override void Update(float simulationDeltaTime)
        {
            LowerAltitude();

            aircraftController.MovementHandler.Turn(aircraftController.Turn);
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
            Vector3 planePosition = aircraftController.MovementHandler.Transform.position;
            Vector3 pointOnApproachLine = Vector3Extensions.FindNearestPointOnLine(aircraftController.AirStripToLandOn.FinalApproach.position,
                aircraftController.AirStripToLandOn.TouchDownPoint.position, planePosition);
            if (Vector3.Distance(planePosition, pointOnApproachLine) > 200)
                return true;
            return false;
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
                aircraftController.AirStripToLandOn.TouchDownPoint.position) < 10);
        }

        private void DoLand()
        {
            stateMachine.ChangeState(aircraftController.StateLanded);
        }

        private void LowerAltitude()
        {
            Vector3 touchDownPosition = aircraftController.AirStripToLandOn.TouchDownPoint.position;
            Vector3 relative = aircraftController.MovementHandler.Transform.InverseTransformPoint(touchDownPosition);
            float targetPitch = Mathf.Atan2(relative.y, relative.z);
            aircraftController.MovementHandler.SetPitch(-targetPitch);
        }
    }
}
