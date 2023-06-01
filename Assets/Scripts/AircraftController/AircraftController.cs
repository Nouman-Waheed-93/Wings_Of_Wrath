using UnityEngine;
using Locomotion;
using Utilities;

namespace AircraftController
{
    public class AircraftController
    {
        private AircraftStateMachine stateMachine;
        public AircraftStateMachine StateMachine { get => stateMachine; }

        private AircraftOrientationController orientationController;
        public AircraftOrientationController OrientationController { get => orientationController; }

        private AircraftMovementHandler movementHandler;
        public AircraftMovementHandler MovementHandler { get => movementHandler; }

        private Transform transform;
        public Transform Transform { get => transform; }

        private Rigidbody rigidbody; //do not remove even if it is not used. Only remove it in production review.

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

        private float turnInput;
        public float TurnInput { get => turnInput; set => turnInput = value; }

        private bool afterBurnerInput;
        public bool AfterBurnerInput { get => afterBurnerInput; set => afterBurnerInput = value; }

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

        public AircraftController(AircraftMovementData movementData, Transform transform, Rigidbody rigidbody, bool startsInAir = false, float startAltitude = 0f, float startSpeed = 0f)
        {
            this.transform = transform;
            this.rigidbody = rigidbody;

            stateMachine = new AircraftStateMachine();
            movementHandler = new AircraftMovementHandler(movementData, transform, rigidbody);
            orientationController = new AircraftOrientationController(movementData, movementHandler, transform.GetChild(0));

            stateOnGround = new OnGround(stateMachine, this);
            stateTakeOff = new TakeOff(stateMachine, this);
            stateInAir = new InAir(stateMachine, this);
            stateFinalApproach = new FinalApproach(stateMachine, this);
            stateTouchDown = new TouchDown(stateMachine, this);
            stateLanded = new Landed(stateMachine, this);

            if (startsInAir) 
            {
                stateMachine.Initialize(stateInAir);
                movementHandler.Initialize(startSpeed, startAltitude);
            } 
            else
            {
                stateMachine.Initialize(stateOnGround);
            }
        }

        public void Update(float simulationDeltaTime)
        {
            stateMachine.currentState.Update(simulationDeltaTime);
            movementHandler.Update(simulationDeltaTime);
            orientationController.Update(simulationDeltaTime);
        }

        public void CalculateAndSetPitch(float targetAltitude, float targetDistance)
        {
            Vector3 targetPosition = transform.position;
            targetPosition.y = targetAltitude;
            Vector3 transformForward = transform.forward;
            transformForward.y = 0;
            transformForward.Normalize();
            targetPosition += transformForward * targetDistance;
            CalculateAndSetPitch(targetPosition);
        }

        public void CalculateAndSetPitch(Vector3 targetPosition)
        {
            Vector3 relative = transform.InverseTransformPoint(targetPosition);
            float targetPitch = -Mathf.Atan2(relative.y, relative.z);
            movementHandler.SetPitch(targetPitch);
        }

        public bool HasDeviatedFromLine(Vector3 lineStart, Vector3 lineEnd, float acceptableDeviation)
        {
            Vector3 planePosition = transform.position;
            Vector3 pointOnApproachLine = Vector3Extensions.FindNearestPointOnLine(lineStart,
                lineEnd, planePosition);
            if (Vector3.Distance(planePosition, pointOnApproachLine) > acceptableDeviation)
                return true;
            return false;
        }

        public void SeekSpeed(float targetSpeed)
        {
            //calculate required throttle
            float requiredThrottle = targetSpeed / movementHandler.AerodynamicMovementData.maxSpeed; 
            //calculate required brakePressure
            float requiredBrakePressure = (movementHandler.CurrSpeed - targetSpeed) * 0.5f;

            movementHandler.SetBrake(requiredBrakePressure);
            movementHandler.SetThrottle(requiredThrottle);
        }

    }
}
