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
            public IAircraft aircraft { get; private set; }
            public IRelativePositionProvider transform { get; private set; }

            private AIStateMachine stateMachine = new AIStateMachine();
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

                float verticalDistance = -leader.Transform.GetRelativePosition(transform.position).z;
                if (verticalDistance > 100)
                    desiredSpeed = aircraft.MovementHandler.AerodynamicMovementData.highAirSpeed;
                else if (verticalDistance > 10)
                    desiredSpeed = aircraft.MovementHandler.AerodynamicMovementData.normalAirSpeed;
                else
                    desiredSpeed = aircraft.MovementHandler.AerodynamicMovementData.lowAirSpeed;

                Vector3 myPositionInTheFormation = myFormationMember.Formation.GetMemberPosition(myFormationMember.PositionIndex) * 10;
                Vector3 targetPosition = leader.Transform.GetGlobalPosition(myPositionInTheFormation);
                float dot = Vector3.Dot(transform.forward, targetPosition - transform.position);
                bool isTargetBehind = dot < 0;
                float distance = Vector3.Distance(transform.position, targetPosition);
                if (isTargetBehind && distance < 50)
                {
                    targetPosition = transform.position + transform.forward;
                }
                TurnTowardsPosition(targetPosition);
            }
        }
    }
}
