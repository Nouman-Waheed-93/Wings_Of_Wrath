using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using FormationSystem;
using Utilities;
using static UnityEngine.ParticleSystem;
using static UnityEngine.GraphicsBuffer;

namespace AircraftController
{
    namespace AircraftAI
    {
        public class AircraftAIController : IAircraftController
        {

            public IAircraft aircraft { get; private set; }
            public IRelativePositionProvider transform { get; private set; }

            private AIStateMachine stateMachine = new AIStateMachine();
            public AIStateMachine StateMachine { get => stateMachine; }

            public StateFollowWaypoints stateFollowWaypoints { get; private set; }
            public StateFollowFormation stateFollowFormation { get; private set; }

            public bool IsAfterBurnerOn => true;

            private float turnInput;
            private float desiredSpeed;

            private Dictionary<IFormationMember, PIDController> formationPIDControllers = new Dictionary<IFormationMember, PIDController>();

            public AircraftAIController(IAircraft aircraft, IRelativePositionProvider transform, Vector3[] wayPoints)
            {
                this.aircraft = aircraft;
                this.transform = transform;
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

                Vector3 myPositionInTheFormation = myFormationMember.Formation.GetMemberPositionSpaced(myFormationMember.PositionIndex);
                Vector3 targetPosition = leader.Transform.GetGlobalPosition(myPositionInTheFormation);

                Vector3 separationForce = CalculateSeparationForce(myFormationMember);

                Vector3 separationDirection = transform.forward * 0.1f + separationForce;

                TurnTowardsPosition(transform.position + separationDirection);
                float separationInput = turnInput;

                Debug.DrawRay(transform.position, separationDirection, Color.cyan);

                //Debug.DrawLine(transform.position, targetPosition, Color.red);
                Arrive(targetPosition);

                targetPosition += leader.Transform.forward * desiredSpeed;

                TurnTowardsPosition(targetPosition);

                turnInput += separationInput;
            }

            private bool IsAccuratelyInFormation(IFormationMember formationMember, IFormationMember leader, Vector3 targetPosition)
            {
                if (Vector3.Distance(formationMember.Transform.position, targetPosition) < formationMember.Formation.spacing)
                {
                    return true;
                }
                return false;
            }

            private Vector3 CalculateSeparationForce(IFormationMember formationMember)
            {
                float separationDistance = formationMember.Formation.spacing;
                float separationStrength = 10.0f; // Strength of the separation force

                Vector3 separationForce = Vector3.zero;

                foreach (IFormationMember target in formationMember.Formation.Members)
                {
                    if (target == formationMember)
                    {
                        continue;
                    }

                    Vector3 relativePos = transform.position - target.Transform.position;
                    Vector3 relativeVel = formationMember.velocity - target.velocity;
                    float distance = relativePos.magnitude;
                    float relativeSpeed = relativeVel.magnitude;

                    if (relativeSpeed == 0)
                        continue;

                    float timeToCollision = -1 * Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);

                    Vector3 separation = relativePos + relativeVel * timeToCollision;
                    float minSeparation = separation.magnitude;

                    float distanceCoefficient = separationDistance / distance;

                    float shortestTime = Mathf.Lerp(0.01f, 10f, distanceCoefficient);
                    float radius = Mathf.Lerp(5f, 15f, distanceCoefficient);
                    if (minSeparation > radius + radius)
                        continue;

                    bool isCollisionHazard = (timeToCollision > 0) && (timeToCollision < shortestTime);
                    if (isCollisionHazard)
                    {
                        Vector3 repulsiveForce = relativePos.normalized / distance;
                        separationForce += repulsiveForce;
                    }
                }

                separationForce *= separationStrength;

                return separationForce;
            }

            private void Arrive(Vector3 targetPosition)
            {
                Vector3 ToPosition = targetPosition - transform.position;

                float distanceAhead = Vector3.Dot(ToPosition, transform.forward);

                if (distanceAhead > 0)
                {
                    float lerpVal = distanceAhead / 50;
                    desiredSpeed = Mathf.Lerp(aircraft.MovementHandler.AerodynamicMovementData.normalAirSpeed, aircraft.MovementHandler.AerodynamicMovementData.highAirSpeed, lerpVal);
                    desiredSpeed = Mathf.Clamp(desiredSpeed, aircraft.MovementHandler.AerodynamicMovementData.lowAirSpeed, aircraft.MovementHandler.AerodynamicMovementData.highAirSpeed);
                    //   Debug.Log("Lerp val is " + lerpVal + " desiredSpeed " + desiredSpeed);
                }
                else
                {
                    float lerpVal = (distanceAhead * -1) / 100;
                    lerpVal = Mathf.Clamp01(lerpVal);
                    lerpVal = 1 - lerpVal;
                    desiredSpeed = Mathf.Lerp(aircraft.MovementHandler.AerodynamicMovementData.lowAirSpeed, aircraft.MovementHandler.AerodynamicMovementData.normalAirSpeed, lerpVal);
                    desiredSpeed = Mathf.Clamp(desiredSpeed, aircraft.MovementHandler.AerodynamicMovementData.lowAirSpeed, aircraft.MovementHandler.AerodynamicMovementData.highAirSpeed);
                    //      Debug.Log("reverse Lerp val is " + lerpVal + " desiredSpeed " + desiredSpeed);
                }
            }
        }
    }
}
