using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Locomotion;

namespace AircraftController
{
    public class AircraftController
    {
        private AircraftStateMachine stateMachine;

        private OnGround stateOnGround;

        public AircraftController()
        {
            stateMachine = new AircraftStateMachine();
            stateOnGround = new OnGround(stateMachine, this);
        }

        public void MoveOnRunway()
        {
            
        }
    }
}
