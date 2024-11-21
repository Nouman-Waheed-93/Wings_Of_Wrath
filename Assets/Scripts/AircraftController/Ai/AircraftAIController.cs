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
				Debug.DrawLine(transform.position, targetPosition, Color.cyan);
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

				targetPosition += leader.Transform.forward * leader.velocity.magnitude;
				TurnTowardsPosition(targetPosition);
			}

			private void CalculateDesiredSpeedToFollowFormation(Vector3 targetPosition, IFormationMember leader)
            {
                IFormationMember myFormationMember = aircraft.formationMember;
                float forwardDistanceToTargetPos = GetDistanceAhead(targetPosition);
                float leaderSpeed = leader.velocity.magnitude;

                Vector3 relativeVelocity = myFormationMember.velocity - leader.velocity;
				float closureSpeed = RelativeVelocityUtility.CalculateClosureSpeed(leader.Transform.position, aircraft.Transform.position, relativeVelocity);// CalculateClosureSpeed(leader.Transform.position, myFormationMember.Transform.position, relativeVelocity);

                float throttleRequiredForTargetSpeed = aircraft.GetRequiredThrottleForSpeed(leaderSpeed);

                float decelerationAtTargetSpeed = Mathf.Lerp(aircraft.MovementHandler.AerodynamicMovementData.maxDeceleration, 0, throttleRequiredForTargetSpeed);
                float distanceThatCanBeCoveredUntilZeroRelSpeed = RelativeVelocityUtility.GetDistanceToReachSpeed(closureSpeed, 0, -decelerationAtTargetSpeed);

				//Guzara if statement below, with guzara jugaar
				if(forwardDistanceToTargetPos < -0.1f)
				{
					desiredSpeed = aircraft.MovementHandler.AerodynamicMovementData.lowAirSpeed;
					return;
                }

                /* If currSpeed is higher than the target speed and the aircraft can reach
                 the target position by normal deceleration
                 {Decelerate} */
                if (aircraft.MovementHandler.CurrSpeed > leaderSpeed &&
                    distanceThatCanBeCoveredUntilZeroRelSpeed > forwardDistanceToTargetPos - 0.1f)
                {
					//--To Do : Set lower desired speed when the zero rel speed distance is too big --//
					desiredSpeed = leaderSpeed;
				}
                else
                {
					desiredSpeed = aircraft.MovementHandler.AerodynamicMovementData.maxSpeed;
                }
                //	desiredSpeed += Random.Range(-0.5f, 0.5f);
                Debug.DrawLine(transform.position, targetPosition, Color.blue);
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

			private float GetDistanceAhead(Vector3 targetPosition)
			{
                Vector3 ToPosition = targetPosition - transform.position;

                float distanceAhead = Vector3.Dot(ToPosition, transform.forward);
				return distanceAhead;
            }

		}
	}
}
