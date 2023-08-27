namespace AircraftController
{
    public class InAir : AircraftState
    {
        public InAir(AircraftStateMachine stateMachine, Aircraft aircraftController) : base(stateMachine, aircraftController)
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
            //Can Fire Weapons
            if (IsInitialApproachDone())
            {
                MoveToFinalApproach();
            }
            aircraftController.SeekSpeed(aircraftController.DesiredSpeed);
            aircraftController.CalculateAndSetPitch(GlobalAircraftControllerSettings.flightAltitude, 100);
            aircraftController.MovementHandler.Turn(aircraftController.TurnInput);
        }

        private bool IsInitialApproachDone()
        {
            return aircraftController.AirStripToLandOn != null;
        }
        private void MoveToFinalApproach()
        {
            stateMachine.ChangeState(aircraftController.StateFinalApproach);
        }
    }
}
