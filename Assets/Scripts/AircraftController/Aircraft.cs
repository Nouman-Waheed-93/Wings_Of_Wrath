using UnityEngine;
using Locomotion;
using Utilities;
using Common;
using FormationSystem;

namespace AircraftController
{
    public class Aircraft : IAircraft, IRelativePositionProvider, IFormationMember
    {
        private AircraftStateMachine stateMachine;
        public AircraftStateMachine StateMachine { get => stateMachine; }

        private AircraftOrientationController orientationController;
        public AircraftOrientationController OrientationController { get => orientationController; }

        private AircraftMovementHandler movementHandler;
        public AircraftMovementHandler MovementHandler { get => movementHandler; }

        private IAircraftController aircraftInputController;
        public IAircraftController AircraftInputController { get => aircraftInputController; set => aircraftInputController = value; }

        private Transform transform;
        public Transform Transform { get => transform; }

        private Rigidbody rigidbody; //do not remove even if it is not used. Only remove it in production review.

        private ISensor[] frontalSensors;
        public ISensor[] FrontalSensors { get; private set; }

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

        float IFormationMember.turnDir { get => aircraftInputController.GetTurn(); }

        Vector3 IFormationMember.velocity { get => rigidbody.velocity; }

        public float TurnInput { get => aircraftInputController.GetTurn(); }
        public float DesiredSpeed { get => aircraftInputController.GetDesiredSpeed(); }

        public bool AfterBurnerInput { get => aircraftInputController.IsAfterBurnerOn; }

        private float throttle;
        public float Throttle
        {
            get => throttle;
            set
            {
                throttle = value;
                movementHandler.SetThrottle(value);
            }
        }

        private Airstrip airstripToLandOn;
        public Airstrip AirStripToLandOn { get => airstripToLandOn; set => airstripToLandOn = value; }

        Vector3 ITransform.position { get => transform.position; set => transform.position = value; }
        Quaternion ITransform.rotation { get => transform.rotation; set => transform.rotation = value; }
        Vector3 ITransform.forward => transform.forward;
        Vector3 ITransform.right => transform.right;

        public IFormationMember formationMember { get => this; }
        int IFormationMember.PositionIndex { get; set; }
        Vector3 IFormationMember.Position { get; set; }
        IRelativePositionProvider IFormationMember.Transform => this;
        public Formation Formation { get; set; }

        public Aircraft(AircraftMovementData movementData, Transform transform, Rigidbody rigidbody, Vector3[] waypoints = null, IAircraftController aircraftController = null, bool startsInAir = false, float startAltitude = 0f, float startSpeed = 0f)
        {
            this.transform = transform;
            this.rigidbody = rigidbody;

            stateMachine = new AircraftStateMachine();
            movementHandler = new AircraftMovementHandler(movementData, transform, rigidbody);
            orientationController = new AircraftOrientationController(movementData, movementHandler, transform.GetChild(0));

            if (aircraftController == null)
            {
                this.aircraftInputController = new AircraftAI.AircraftAIController(this, this, waypoints);
            }
            else
            {
                this.aircraftInputController = aircraftController;
            }

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

        public Aircraft SetSensors(ISensor[] sensors)
        {
            this.frontalSensors = sensors;
            return this;
        }

        public void Update(float simulationDeltaTime)
        {
            aircraftInputController.Update(simulationDeltaTime);
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
            float requiredThrottle = GetRequiredThrottleForSpeed(targetSpeed);
            movementHandler.SetThrottle(requiredThrottle);
        }

        public float GetRequiredThrottleForSpeed(float targetSpeed)
        {
            return targetSpeed / movementHandler.AerodynamicMovementData.maxSpeed;
        }

        //Remove if the break is already handled elsewhere
        private float CalculateRequiredBrakePressure(float targetSpeed)
        {
            float speedDifference = movementHandler.CurrSpeed - targetSpeed;
            float clampedSpeedDifference = Mathf.Clamp(speedDifference, 10, 30);
            float requiredBrakePressure = clampedSpeedDifference / 20f;
            return requiredBrakePressure;
        }

        public float GetSpeedToFollow(Vector3 targetPosition, IFormationMember toFollow)
        {
            IFormationMember myFormationMember = formationMember;
            float forwardDistanceToTargetPos = GetDistanceAhead(targetPosition);
            float leaderSpeed = toFollow.velocity.magnitude;

            Vector3 relativeVelocity = myFormationMember.velocity - toFollow.velocity;
            float closureSpeed = RelativeVelocityUtility.CalculateClosureSpeed(toFollow.Transform.position, Transform.position, relativeVelocity);// CalculateClosureSpeed(leader.Transform.position, myFormationMember.Transform.position, relativeVelocity);

            float throttleRequiredForTargetSpeed = GetRequiredThrottleForSpeed(leaderSpeed);

            float decelerationAtTargetSpeed = Mathf.Lerp(MovementHandler.AerodynamicMovementData.maxDeceleration, 0, throttleRequiredForTargetSpeed);
            float distanceThatCanBeCoveredUntilZeroRelSpeed = RelativeVelocityUtility.GetDistanceToReachSpeed(closureSpeed, 0, -decelerationAtTargetSpeed);

            //Guzara if statement below, with guzara jugaar
            if (forwardDistanceToTargetPos < -1f)
            {
                return movementHandler.AerodynamicMovementData.lowAirSpeed;
                MovementHandler.SetBrake(1);
            }

            /* If currSpeed is higher than the target speed and the aircraft can reach
             the target position by normal deceleration
             {Decelerate} */
            if (MovementHandler.CurrSpeed > leaderSpeed &&
                distanceThatCanBeCoveredUntilZeroRelSpeed > forwardDistanceToTargetPos - 0.1f)
            {
                //--To Do : Set lower desired speed when the zero rel speed distance is too big --//

                if (distanceThatCanBeCoveredUntilZeroRelSpeed - forwardDistanceToTargetPos > 3)
                {
                    //hit the brakes
                    MovementHandler.SetBrake(1);
                }
                else
                {
                    MovementHandler.SetBrake(0);
                }
                return leaderSpeed;
            }
            else
            {
                MovementHandler.SetBrake(0);
                return movementHandler.AerodynamicMovementData.maxSpeed;
            }
            //	desiredSpeed += Random.Range(-0.5f, 0.5f);
            Debug.DrawLine(transform.position, targetPosition, Color.blue);
        }

        /// <summary>
        /// This method returns true if there is an obstacle/ object
        /// right in front.
        /// </summary>
        /// <returns></returns>
        public bool IsCollisionHazardAhead()
        {
            for(int i = 0; i< frontalSensors.Length; i++)
            {
                if (frontalSensors[i].HasSomethingInFront)
                {
                    return true;
                }
            }
            return false;
        }

        private float GetDistanceAhead(Vector3 targetPosition)
        {
            Vector3 ToPosition = targetPosition - transform.position;

            float distanceAhead = Vector3.Dot(ToPosition, transform.forward);
            return distanceAhead;
        }

        Vector3 IRelativePositionProvider.GetRelativePosition(Vector3 point)
        {
            return transform.InverseTransformPoint(point);
        }

        Vector3 IRelativePositionProvider.GetGlobalPosition(Vector3 localPosition)
        {
            return transform.TransformPoint(localPosition);
        }
    }
}
