using UnityEngine;

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

        }

        public override void Exit()
        {
            //Play Airborn dialog sequence
        }
    }
}
