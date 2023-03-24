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
            return false;
        }

        private bool CanAbort()
        {
            return false;
        }

        private void DoAbort()
        {

        }

        private void DoCrash()
        {

        }

        private bool IsTouchDownDone()
        {
            return false;
        }

        private void DoLand()
        {
            stateMachine.ChangeState(aircraftController.StateLanded);
        }
    }
}
