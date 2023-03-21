using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Locomotion;

namespace AircraftController
{
    public class AircraftController
    {
        private AircraftStateMachine stateMachine;
     
        private AerodynamicMovementHandler movementHandler;
        public AerodynamicMovementHandler MovementHandler { get => movementHandler; }

        private OnGround stateOnGround;
        public OnGround StateOnGround { get => stateOnGround; }

        private TakeOff stateTakeOff;
        public TakeOff StateTakeOff { get => stateTakeOff; }

        private InAir stateInAir;
        public InAir StateInAir { get => stateInAir; }

        private float turn;
        public float Turn { get => turn; set => turn = value; }

        private bool isThrottleOn;
        public bool IsThrottleOn { get => isThrottleOn; set => isThrottleOn = value; }

        public AircraftController(AerodynamicMovementHandler movementHandler)
        {
            stateMachine = new AircraftStateMachine();
            this.movementHandler = movementHandler;
            stateOnGround = new OnGround(stateMachine, this);
            stateMachine.Initialize(stateOnGround);
            stateTakeOff = new TakeOff(stateMachine, this);
            stateInAir = new InAir(stateMachine, this);
        }

        public void Update(float simulationDeltaTime)
        {
            stateMachine.currentState.Update(simulationDeltaTime);
        }

    }
}
