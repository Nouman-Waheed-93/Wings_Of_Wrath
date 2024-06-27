using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using FormationSystem;
using Utilities;
using static UnityEngine.ParticleSystem;

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

                Vector3 separationForce = CalculateSeparationForce(myFormationMember) * myFormationMember.Formation.spacing;

                TurnTowardsPosition(transform.position + separationForce);
                float separationTurnInput = turnInput;

                Debug.DrawRay(transform.position, separationForce, Color.cyan);
                
            //    Debug.DrawLine(transform.position, targetPosition, Color.red);
                Arrive(targetPosition);

                targetPosition += leader.Transform.forward * desiredSpeed;
                
                //Debug.DrawLine(transform.position, targetPosition, Color.green);

                TurnTowardsPosition(targetPosition);

                turnInput += separationTurnInput;
            }

            private Vector3 CalculateSeparationForce(IFormationMember myFormationMember)
            {
                float separationDistance = myFormationMember.Formation.spacing; // Desired separation distance
                float separationStrength = 10.0f; // Strength of the separation force

                Vector3 separationForce = Vector3.zero;

                foreach (IFormationMember member in myFormationMember.Formation.Members)
                {
                    if (member != myFormationMember)
                    {
                        Vector3 toMember = myFormationMember.Transform.position - member.Transform.position;
                        float distance = toMember.magnitude;

                        if (distance < separationDistance)
                        {
                            float repulsiveForceFactor = Mathf.Lerp(1f, 0f, distance / separationDistance);
                            if(repulsiveForceFactor > 1)
                            {
                                Debug.Log($"REpulsive Factor : {repulsiveForceFactor}");
                            }
                            // Calculate the repulsive force inversely proportional to the distance
                            Vector3 repulsiveForce = toMember.normalized / distance;
                            separationForce += repulsiveForce * repulsiveForceFactor;
                        }
                    }
                }

                // Scale the separation force by the desired strength
                separationForce *= separationStrength;

                return (transform.forward * 0.1f) + separationForce;
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
