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

        #region States
        private OnGround stateOnGround;
        public OnGround StateOnGround { get => stateOnGround; }

        private TakeOff stateTakeOff;
        public TakeOff StateTakeOff { get => stateTakeOff; }

        private InAir stateInAir;
        public InAir StateInAir { get => stateInAir; }

        private FinalApproach stateFinalApproach;
        public FinalApproach StateFinalApproach { get => stateFinalApproach; }

        private TouchDown stateTouchDown;
        public TouchDown StateTouchDown { get => stateTouchDown; }

        private Landed stateLanded;
        public Landed StateLanded { get => stateLanded; }
        #endregion

        private float turn;
        public float Turn { get => turn; set => turn = value; }

        private float throttle;
        public float Throttle { get => throttle; set => throttle = value; }

        private Airstrip airstripToLandOn;
        public Airstrip AirStripToLandOn { get => airstripToLandOn; set => airstripToLandOn = value; }

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
