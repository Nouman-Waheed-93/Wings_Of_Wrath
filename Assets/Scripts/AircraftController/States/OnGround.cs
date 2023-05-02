using Locomotion;

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
            //first screen touch -> engine start
            //if screen touched -> afterburner
            //on screen touch lifted -> dry thrust
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
