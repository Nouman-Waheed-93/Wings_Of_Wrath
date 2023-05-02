using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Locomotion;

namespace AircraftController
{
    public class AircraftController
    {
        private AircraftStateMachine stateMachine;

        private AircraftOrientationController orientationController;
        public AircraftOrientationController OrientationController { get => orientationController; }

        private AerodynamicMovementHandler movementHandler;
        public AerodynamicMovementHandler MovementHandler { get => movementHandler; }

        private Transform transform;
        private Rigidbody rigidbody;

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
        public float Throttle { 
            get => throttle; 
            set 
            { 
                throttle = value;
                movementHandler.SetThrottle(value);
            } 
        }

        private Airstrip airstripToLandOn;
        public Airstrip AirStripToLandOn { get => airstripToLandOn; set => airstripToLandOn = value; }

        public AircraftController(AerodynamicMovementData movementData, Transform transform, Rigidbody rigidbody)
        {
            this.transform = transform;
            this.rigidbody = rigidbody;

            stateMachine = new AircraftStateMachine();
            movementHandler = new AerodynamicMovementHandler(movementData, transform, rigidbody);
            orientationController = new AircraftOrientationController(movementHandler, transform.GetChild(0));

            stateOnGround = new OnGround(stateMachine, this);
            stateMachine.Initialize(stateOnGround);
            stateTakeOff = new TakeOff(stateMachine, this);
            stateInAir = new InAir(stateMachine, this);
            stateFinalApproach = new FinalApproach(stateMachine, this);
            stateTouchDown = new TouchDown(stateMachine, this);
            stateLanded = new Landed(stateMachine, this);
        }

        public void Update(float simulationDeltaTime)
        {
            stateMachine.currentState.Update(simulationDeltaTime);
            movementHandler.Update(simulationDeltaTime);
            orientationController.Update(simulationDeltaTime);
        }

    }
}
