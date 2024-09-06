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

			private float altitudeOffset;
			public float AltitudeOffset { get => altitudeOffset; }

			public StateFollowWaypoints stateFollowWaypoints { get; private set; }
			public StateFollowFormation stateFollowFormation { get; private set; }

			public bool IsAfterBurnerOn => true;

			private float turnInput;
			private float desiredSpeed;

			private RelativeVelocityCalculator relativeVelToLeader;

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

				if(relativeVelToLeader != null)
				{
                    relativeVelToLeader.Update(simulationDeltaTime);
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

				Vector3 myPositionInTheFormation = myFormationMember.Formation.GetMemberPositionSpaced(myFormationMember.PositionIndex);
				altitudeOffset = myPositionInTheFormation.y;
				Vector3 targetPosition = leader.Transform.GetGlobalPosition(myPositionInTheFormation);

                if (relativeVelToLeader == null)
                {
                    relativeVelToLeader = new RelativeVelocityCalculator(transform, leader.Transform);
                }

                Arrive(targetPosition); //This arrive is not so good because, it does not take into account the relative Velocity
				
				targetPosition += leader.Transform.forward * desiredSpeed;
				TurnTowardsPosition(targetPosition);
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

			private void Arrive(Vector3 targetPosition)
			{
				Vector3 ToPosition = targetPosition - transform.position;

				float distanceAhead = Vector3.Dot(ToPosition, transform.forward);

				if (distanceAhead > 0)
				{
					float lerpVal = distanceAhead / 50;
					desiredSpeed = Mathf.Lerp(aircraft.MovementHandler.AerodynamicMovementData.normalAirSpeed, aircraft.MovementHandler.AerodynamicMovementData.highAirSpeed, lerpVal);
                    //	desiredSpeed = Mathf.Clamp(desiredSpeed, aircraft.MovementHandler.AerodynamicMovementData.lowAirSpeed, aircraft.MovementHandler.AerodynamicMovementData.highAirSpeed);
                    //   Debug.Log("Lerp val is " + lerpVal + " desiredSpeed " + desiredSpeed);
                    AdjustDesiredVelocityAccordingToClosureSpeed();
                }
				else
				{
					float lerpVal = (distanceAhead * -1) / 100;
					lerpVal = Mathf.Clamp01(lerpVal);
					lerpVal = 1 - lerpVal;
					desiredSpeed = Mathf.Lerp(aircraft.MovementHandler.AerodynamicMovementData.lowAirSpeed, aircraft.MovementHandler.AerodynamicMovementData.normalAirSpeed, lerpVal);
					//desiredSpeed = Mathf.Clamp(desiredSpeed, aircraft.MovementHandler.AerodynamicMovementData.lowAirSpeed, aircraft.MovementHandler.AerodynamicMovementData.highAirSpeed);
					//      Debug.Log("reverse Lerp val is " + lerpVal + " desiredSpeed " + desiredSpeed);
				}

            }
		
			private void AdjustDesiredVelocityAccordingToClosureSpeed()
			{
				Debug.Log($"closureSpeed {relativeVelToLeader.ClosureSpeed}");

                if (relativeVelToLeader.ClosureSpeed == 0)
                {
                    return;
                }

				float differential = desiredSpeed / -relativeVelToLeader.ClosureSpeed;
				Debug.Log($"Differential {differential}");
				desiredSpeed *= differential * 0.1f;
				desiredSpeed = Mathf.Clamp(desiredSpeed, aircraft.MovementHandler.AerodynamicMovementData.lowAirSpeed, aircraft.MovementHandler.AerodynamicMovementData.highAirSpeed);
			}
		}
	}
}
