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

                //Debug.DrawLine(transform.position, targetPosition, Color.red);
                Arrive(targetPosition);

                float distance = Vector3.Distance(transform.position, targetPosition);
                targetPosition += leader.Transform.forward * desiredSpeed;
                //Debug.DrawLine(transform.position, targetPosition, Color.green);

                TurnTowardsPosition(targetPosition);

                if (distance < myFormationMember.Formation.spacing * 1.1f)
                {
                    if (myFormationMember.Position.x > leader.Position.x)
                    {
                        if (leader.turnDir > 0.5f)
                        {
                            turnInput = leader.turnDir * 1.2f;
                            desiredSpeed = aircraft.MovementHandler.AerodynamicMovementData.lowAirSpeed;
                            Debug.DrawLine(transform.position, transform.forward * 100, Color.cyan);
                        }
                    }
                    else
                    {
                        if (leader.turnDir < -0.5f)
                        {
                            turnInput = leader.turnDir * 1.2f;
                            desiredSpeed = aircraft.MovementHandler.AerodynamicMovementData.lowAirSpeed;
                            Debug.DrawLine(transform.position, transform.forward * 100, Color.cyan);
                        }
                    }
                }
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
