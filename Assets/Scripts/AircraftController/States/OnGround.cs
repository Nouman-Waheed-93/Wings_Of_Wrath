namespace AircraftController
{
    public class OnGround : State
    {
        public OnGround(AircraftStateMachine stateMachine, AircraftController aircraftController): base(stateMachine, aircraftController)
        {
        }

        public override void Enter()
        {
            //Play clear for take off dialog sequence
        }

        public override void Update(float simulationTime)
        {
            float throttle = aircraftController.IsThrottleOn ? 1 : 0;
            aircraftController.MovementHandler.SetThrottle(throttle);
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
