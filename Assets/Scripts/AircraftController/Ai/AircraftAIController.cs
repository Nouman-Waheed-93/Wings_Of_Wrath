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

            public bool IsAfterBurnerOn => true;

            private float turnInput;
            private float desiredSpeed;

            private float speedError;
            private float errorSetTime;

            public AircraftAIController(IAircraft aircraft, IRelativePositionProvider transform, Vector3[] wayPoints)
            {
                this.aircraft = aircraft;
                this.transform = transform;
                stateFollowWaypoints = new StateFollowWaypoints(stateMachine, this, wayPoints);
                stateFollowFormation = new StateFollowFormation(stateMachine, this);
                stateMachine.Initialize(stateFollowWaypoints);
            }

            private void RandomizeSpeedError()
            {
                speedError = Random.Range(-2, 2);
            }

            public void Update(float simulationDeltaTime)
            {
                stateMachine.currentState.Update(simulationDeltaTime);
                errorSetTime += simulationDeltaTime;
                if(errorSetTime > 1)
                {
                    errorSetTime = 0;
                    RandomizeSpeedError();
                }
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

                //float dot = Vector3.Dot(transform.forward, targetPosition - transform.position);
                //bool isTargetBehind = dot < -0.75f;
                float distance = Vector3.Distance(transform.position, targetPosition);
                //if (isTargetBehind && distance < 100)
                //{
                //    targetPosition = transform.position + transform.forward;
                // //   desiredSpeed = aircraft.MovementHandler.AerodynamicMovementData.lowAirSpeed;
                //}

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

                float distanceAhead = Vector3.Dot(ToPosition, transform.forward);

                if (distanceAhead > 0)
                {
                    float lerpVal = distanceAhead / 200;
                    desiredSpeed = Mathf.Lerp(aircraft.MovementHandler.AerodynamicMovementData.normalAirSpeed, aircraft.MovementHandler.AerodynamicMovementData.highAirSpeed, lerpVal);
                    desiredSpeed += speedError;
                    desiredSpeed = Mathf.Clamp(desiredSpeed, aircraft.MovementHandler.AerodynamicMovementData.lowAirSpeed, aircraft.MovementHandler.AerodynamicMovementData.highAirSpeed);
                    Debug.Log("Lerp val is " + lerpVal + " desiredSpeed " + desiredSpeed);
                }
                else
                {
                    float lerpVal = (distanceAhead * -1) / 100;
                    lerpVal = Mathf.Clamp01(lerpVal);
                    lerpVal = 1 - lerpVal;
                    desiredSpeed = Mathf.Lerp(aircraft.MovementHandler.AerodynamicMovementData.lowAirSpeed, aircraft.MovementHandler.AerodynamicMovementData.normalAirSpeed, lerpVal);
                    desiredSpeed += speedError;
                    desiredSpeed = Mathf.Clamp(desiredSpeed, aircraft.MovementHandler.AerodynamicMovementData.lowAirSpeed, aircraft.MovementHandler.AerodynamicMovementData.highAirSpeed);
                    Debug.Log("reverse Lerp val is " + lerpVal + " desiredSpeed " + desiredSpeed);
                }
            }
        }
    }
}
