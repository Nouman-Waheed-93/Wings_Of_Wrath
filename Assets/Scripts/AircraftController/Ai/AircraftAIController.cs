using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using FormationSystem;
using Utilities;

namespace AircraftController
{
    namespace AircraftAI
    {
        public class AircraftAIController : IAircraftController
        {
            public IAircraft aircraft { get; private set; }
            public IRelativePositionProvider transform { get; private set; }

            private AIStateMachine stateMachine = new AIStateMachine();
            public StateFollowWaypoints stateFollowWaypoints { get; private set; }
            public StateFollowFormation stateFollowFormation { get; private set; }

            private PIDController speedPIDController;
            public PIDController SpeedPIDController { get => speedPIDController; }

            public bool IsAfterBurnerOn => true;

            private float turnInput;
            private float desiredSpeed;

            public AircraftAIController(IAircraft aircraft, IRelativePositionProvider transform, Vector3[] wayPoints)
            {
                this.aircraft = aircraft;
                this.transform = transform;
                speedPIDController = new PIDController();
                speedPIDController.minimum = -1;
                speedPIDController.maximum = 0;
                stateFollowWaypoints = new StateFollowWaypoints(stateMachine, this, wayPoints);
                stateFollowFormation = new StateFollowFormation(stateMachine, this);
                stateMachine.Initialize(stateFollowWaypoints);
            }

            public void Update(float simulationDeltaTime)
            {
                stateMachine.currentState.Update(simulationDeltaTime);
            }

            public float GetDesiredSpeed()
            {
                return desiredSpeed;
            }

            public float GetTurn()
            {
                return turnInput;
            }

            public void SetThrottleNormal()
            {
                desiredSpeed = aircraft.MovementHandler.AerodynamicMovementData.normalAirSpeed;
            }

            public void TurnTowardsPosition(Vector3 targetPosition)
            {
                Vector3 relative = transform.GetRelativePosition(targetPosition);
                turnInput = Mathf.Atan2(relative.x, relative.z) / (Mathf.PI * 0.5f);
            }

            public void FollowFormation()
            {
                IFormationMember leader = aircraft.formationMember.Formation.leader;
                IFormationMember myFormationMember = aircraft.formationMember;

                Vector3 myPositionInTheFormation = myFormationMember.Formation.GetMemberPosition(myFormationMember.PositionIndex) * 30;
                Vector3 targetPosition = leader.Transform.GetGlobalPosition(myPositionInTheFormation);

                Debug.DrawLine(transform.position, targetPosition, Color.red);
                Arrive(targetPosition);

                float dot = Vector3.Dot(transform.forward, targetPosition - transform.position);
                bool isTargetBehind = dot < -0.75f;
                float distance = Vector3.Distance(transform.position, targetPosition);
                if (isTargetBehind && distance < 100)
                {
                    targetPosition = transform.position + transform.forward;
                 //   desiredSpeed = aircraft.MovementHandler.AerodynamicMovementData.lowAirSpeed;
                }

                targetPosition += leader.Transform.forward * desiredSpeed;

              //  Debug.DrawLine(transform.position, targetPosition, Color.blue);

                TurnTowardsPosition(targetPosition);
                distance = Mathf.Clamp(distance, 0, 50);
                float turnDistanceMultiplier = Mathf.Clamp01(distance * 0.1f);
                turnInput *= turnDistanceMultiplier;
            }

            private void Arrive(Vector3 targetPosition)
            {
                Vector3 ToPosition = targetPosition - transform.position;

                float distanceAhead = Vector3.Dot(ToPosition, transform.forward) / 20;
                float pidVal = speedPIDController.Seek(0, distanceAhead);
                desiredSpeed = pidVal * -1 * aircraft.MovementHandler.AerodynamicMovementData.highAirSpeed;
                desiredSpeed = Mathf.Clamp(desiredSpeed, aircraft.MovementHandler.AerodynamicMovementData.lowAirSpeed, aircraft.MovementHandler.AerodynamicMovementData.highAirSpeed);
                Debug.Log("PIDVal " + pidVal + "Dist : " + distanceAhead + " desiredSpeed : " + desiredSpeed);
            }
        }
    }
}
