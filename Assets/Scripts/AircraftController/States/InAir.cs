using UnityEngine;

namespace AircraftController
{
    public class InAir : State
    {
        public InAir(AircraftStateMachine stateMachine, AircraftController aircraftController) : base(stateMachine, aircraftController)
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
            aircraftController.SeekSpeed(aircraftController.MovementHandler.AerodynamicMovementData.normalAirSpeed);
            aircraftController.CalculateAndSetPitch(GlobalAircraftControllerSettings.flightAltitude, 100);
            aircraftController.MovementHandler.Turn(aircraftController.Turn);
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
