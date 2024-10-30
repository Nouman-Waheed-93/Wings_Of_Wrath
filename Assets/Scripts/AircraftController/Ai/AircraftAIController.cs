using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using FormationSystem;
using Utilities;
using static UnityEngine.ParticleSystem;
using Locomotion;
using UnityEngine.UIElements;

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

			private float altitudeOffset;
			public float AltitudeOffset { get => altitudeOffset; }

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
				altitudeOffset = myPositionInTheFormation.y;
				Vector3 targetPosition = leader.Transform.GetGlobalPosition(myPositionInTheFormation);

				CalculateDesiredSpeedToFollowFormation(targetPosition, leader);
				
				targetPosition += leader.Transform.forward * desiredSpeed;
				TurnTowardsPosition(targetPosition);
			}

			private void CalculateDesiredSpeedToFollowFormation(Vector3 targetPosition, IFormationMember leader)
            {
                IFormationMember myFormationMember = aircraft.formationMember;
                float distanceToTargetPos = Vector3.Distance(targetPosition, transform.position);
                float leaderSpeed = leader.velocity.magnitude;

                Vector3 relativeVelocity = myFormationMember.velocity - leader.velocity;
				float closureSpeed = RelativeVelocityUtility.CalculateClosureSpeed(leader.Transform.position, aircraft.Transform.position, relativeVelocity);// CalculateClosureSpeed(leader.Transform.position, myFormationMember.Transform.position, relativeVelocity);

                float throttleRequiredForTargetSpeed = aircraft.GetRequiredThrottleForSpeed(leaderSpeed);

                float decelerationAtTargetSpeed = Mathf.Lerp(aircraft.MovementHandler.AerodynamicMovementData.maxDeceleration, 0, throttleRequiredForTargetSpeed);
                float distanceThatCanBeCoveredUntilZeroRelSpeed = RelativeVelocityUtility.GetDistanceToReachSpeed(closureSpeed, 0, decelerationAtTargetSpeed);

                // If currSpeed is higher than the target speed and the aircraft can reach
                // the target position by normal deceleration
                // {Decelerate}
                if (aircraft.MovementHandler.CurrSpeed > leaderSpeed &&
                    Mathf.Abs(distanceToTargetPos - distanceThatCanBeCoveredUntilZeroRelSpeed) < 0.1f)
                {
                    desiredSpeed = leaderSpeed;
                }
                // else
                // calculate the high speed that we need to achieve to close the distance.
                // (we should not use the max speed
                // because, the distance might not be large enough
                // that we would need to achieve the max speed to close the distance)
                //We can calculate the distance (D1) it will take to reach the high speed.
                //We know the max speed and the target speed when at the target location. We also know the deceleration rate.
                //We can calculate the distance (D2) it will take to reach target speed from the max speed.
                //When we subtract these two distances (D1, D2) from the actual distance. We get the distance that we will be traveling
                // on the max speed.
                else
                {
                    /* ==========================Start Your work here=====================================
					 */
                }
            }


            /// <summary>
            /// Not using this method for now. Because, it is only creating more chaos
            /// </summary>
            /// <param name="myFormationMember"></param>
            /// <returns></returns>
            private Vector3 CalculateSeparationForce(IFormationMember myFormationMember)
			{
				float separationDistance = myFormationMember.Formation.spacing; // Desired separation distance
				float separationStrength = 1.0f; // Strength of the separation force

				Vector3 separationForce = Vector3.zero;

				foreach (IFormationMember member in myFormationMember.Formation.Members)
				{
					if (member != myFormationMember)
					{
						Vector3 toMember = myFormationMember.Transform.position - member.Transform.position;
						float distance = toMember.magnitude;

						if (distance < separationDistance)
						{
							// Calculate the repulsive force inversely proportional to the distance
							Vector3 repulsiveForce = toMember.normalized / distance;
							separationForce += repulsiveForce;
						}
					}
				}

				// Scale the separation force by the desired strength
				separationForce *= separationStrength;

				return separationForce;
			}

		}
	}
}
