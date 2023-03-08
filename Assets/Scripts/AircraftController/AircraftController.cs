using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Locomotion;

namespace AircraftController
{
    public class AircraftController
    {
        private AircraftStateMachine stateMachine;
        private MovementHandler movementHandler;

        private OnGround stateOnGround;

        public AircraftController(MovementHandler movementHandler)
        {
            stateMachine = new AircraftStateMachine();
            stateOnGround = new OnGround(stateMachine, this);
            this.movementHandler = movementHandler;
        }

        public void MoveOnRunway()
        {
            
        }
    }
}
