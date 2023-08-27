using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using FormationSystem;

namespace AircraftController
{
    namespace AircraftAI
    {
        public class AircraftAIController : IAircraftController
        {
            private IAircraft aircraft;
            public IRelativePositionProvider transform { get; private set; }

            private AIStateMachine stateMachine = new AIStateMachine();
            private StateFollowWaypoints stateFollowWaypoints;

            public bool IsAfterBurnerOn => true;

            private float turnInput;
            private float desiredSpeed;

            public AircraftAIController(IAircraft aircraft, IRelativePositionProvider transform, Vector3[] wayPoints)
            {
                this.aircraft = aircraft;
                this.transform = transform;
                stateFollowWaypoints = new StateFollowWaypoints(stateMachine, this, wayPoints);
                stateMachine.Initialize(stateFollowWaypoints);
            }

            public void Update(float simulationDeltaTime)
            {
                stateMachine.currentState.Update(simulationDeltaTime);
            }

            public float GetDesiredSpeed()
            {
                return desiredSpeed;

                if (aircraft.formationMember.Formation != null && aircraft.formationMember.PositionIndex != 0)
                {
                    float verticalDistance = -aircraft.formationMember.Formation.leader.Transform.GetRelativePosition(transform.position).z;
                    float desiredSpeed = aircraft.MovementHandler.AerodynamicMovementData.normalAirSpeed;
                    if (verticalDistance > 100)
                        desiredSpeed = aircraft.MovementHandler.AerodynamicMovementData.highAirSpeed;
                    else if (verticalDistance > 20)
                        desiredSpeed = aircraft.MovementHandler.AerodynamicMovementData.normalAirSpeed;
                    else
                        desiredSpeed = aircraft.MovementHandler.AerodynamicMovementData.lowAirSpeed;

                    return desiredSpeed;
                }
                return aircraft.MovementHandler.AerodynamicMovementData.normalAirSpeed;
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
        }
    }
}
