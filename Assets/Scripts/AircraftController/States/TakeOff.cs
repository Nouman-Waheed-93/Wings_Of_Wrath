namespace AircraftController
{
    public class TakeOff : AircraftState
    {
        public TakeOff(AircraftStateMachine stateMachine, Aircraft aircraftController) : base(stateMachine, aircraftController)
        {
        }

        public override void Enter()
        {
            //Play Airborne audio
        }

        public override void Exit()
        {

        }

        public override void Update(float simulationDeltaTime)
        {
            aircraftController.CalculateAndSetPitch(GlobalAircraftControllerSettings.flightAltitude, 300);
            if(aircraftController.MovementHandler.Altitude > GlobalAircraftControllerSettings.retractGearAltitude)
            {
                //retract gear
            }
            if(aircraftController.MovementHandler.Altitude >= GlobalAircraftControllerSettings.airborneAltitude)
            {
                stateMachine.ChangeState(aircraftController.StateInAir);
            }
        }
    }
}
