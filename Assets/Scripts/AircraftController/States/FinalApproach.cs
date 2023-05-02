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
            LowerAltitude();
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
            Vector3 planePosition = aircraftController.MovementHandler.Transform.position;
            Vector3 pointOnApproachLine = Vector3Extensions.FindNearestPointOnLine(aircraftController.AirStripToLandOn.InitialApproach.position,
                aircraftController.AirStripToLandOn.FinalApproach.position, planePosition);
            if (Vector3.Distance(planePosition, pointOnApproachLine) > 100)
                return true;
            return false;
        }

        private void AbortLanding()
        {
            aircraftController.MovementHandler.SetBrake(0);
            aircraftController.Throttle = 1;
            stateMachine.ChangeState(aircraftController.StateInAir);
            aircraftController.AirStripToLandOn = null;
            //retract landing gear
        }

        private bool IsFinalApproachDone()
        {
            return (Vector3.Distance(aircraftController.MovementHandler.Transform.position,
                aircraftController.AirStripToLandOn.FinalApproach.position) < 10);
        }

        private void MoveToTouchDown()
        {
            stateMachine.ChangeState(aircraftController.StateTouchDown);
        }
        
        private void LowerAltitude()
        {
            Vector3 finalApproach = aircraftController.AirStripToLandOn.FinalApproach.position;
            Vector3 transformForward = aircraftController.MovementHandler.Transform.forward;
            transformForward.y = 0;
            transformForward.Normalize();
            float distance = Vector3.Distance(aircraftController.MovementHandler.Transform.position, finalApproach);
            Vector3 targetPosition = transformForward * distance + new Vector3(0, finalApproach.y, 0);
            Vector3 relative = aircraftController.MovementHandler.Transform.InverseTransformPoint(targetPosition);
            float targetPitch = Mathf.Atan2(relative.y, relative.z);
            aircraftController.MovementHandler.SetPitch(-targetPitch);
        }
    }
}
