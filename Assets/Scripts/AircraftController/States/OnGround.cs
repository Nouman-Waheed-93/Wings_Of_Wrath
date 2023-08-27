namespace AircraftController
{
    public class OnGround : AircraftState
    {
        private bool isEngineStarted;
        public OnGround(AircraftStateMachine stateMachine, Aircraft aircraftController): base(stateMachine, aircraftController)
        {
        }

        public override void Enter()
        {
            isEngineStarted = false;
            //Play clear for take off dialog sequence
        }

        public override void Update(float simulationTime)
        {
            //first screen touch -> engine start
            if (!isEngineStarted)
            {
                if (aircraftController.AfterBurnerInput)
                {
                    aircraftController.Throttle = 1;
                }
            }
            else
            {
                //if screen touched -> afterburner
                if (aircraftController.AfterBurnerInput)
                {
                    aircraftController.Throttle = 1;
                }
                //on screen touch lifted -> dry thrusts
                else
                {
                    aircraftController.Throttle = 0.7f;
                }
            }
            if(aircraftController.MovementHandler.CurrSpeed > aircraftController.MovementHandler.AerodynamicMovementData.takeOffSpeed)
            {
                stateMachine.ChangeState(aircraftController.StateTakeOff);
            }
        }

        public override void Exit()
        {
        }
    }
}
