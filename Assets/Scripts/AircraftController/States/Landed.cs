using System.Collections;
using UnityEngine;

namespace AircraftController
{
    public class Landed : State
    {
        public Landed(AircraftStateMachine stateMachine, AircraftController aircraftController) 
            : base(stateMachine, aircraftController)
        {
        }

        public override void Enter()
        {
            //Play the particle effect
            //Play the screeching tyres SFX
        }

        public override void Exit()
        {
        }

        public override void Update(float simulationDeltaTime)
        {
            //Slow down to a halt
            //Automatically turn the aircraft to keep it aligned with the runway
        }
    }
}