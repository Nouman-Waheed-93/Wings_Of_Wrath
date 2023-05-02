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
        }

        public override void Exit()
        {
        }

        public override void Update(float simulationDeltaTime)
        {
            LowerAltitude();

            if (IsAbortIntended())
            {
                if (CanAbort())
                    DoAbort();
                else
                    DoCrash();
            }
            else if (IsTouchDownDone())
            {
                DoLand();
            }
        }

        private bool IsAbortIntended()
        {
            Vector3 planePosition = Vector3.zero;
            Vector3 pointOnApproachLine = Vector3Extensions.FindNearestPointOnLine(aircraftController.AirStripToLandOn.FinalApproach.position,
                aircraftController.AirStripToLandOn.TouchDownPoint.position, planePosition);
            if (Vector3.Distance(planePosition, pointOnApproachLine) > 100)
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
            Vector3 transformForward = aircraftController.MovementHandler.Transform.forward;
            transformForward.y = 0;
            transformForward.Normalize();
            float distance = Vector3.Distance(aircraftController.MovementHandler.Transform.position, touchDownPosition);
            Vector3 targetPosition = transformForward * distance + new Vector3(0, touchDownPosition.y, 0);
            Vector3 relative = aircraftController.MovementHandler.Transform.InverseTransformPoint(targetPosition);
            float targetPitch = Mathf.Atan2(relative.y, relative.z);
            aircraftController.MovementHandler.SetPitch(-targetPitch);
        }
    }
}
