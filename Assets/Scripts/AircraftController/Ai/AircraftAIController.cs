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
				//Predict my position in the formation based on leader's angular velocity
				Vector3 targetPosition = leader.Transform.GetGlobalPosition(myPositionInTheFormation);

				desiredSpeed = aircraft.GetSpeedToFollow(targetPosition, leader);


                //Todo: better prediction time calculation. Hint: We can calculate the prediction time based on the leader's angular velocity
                float predictionTime = 1;
				if(leader.angularVelocity.y > 0.5f && myPositionInTheFormation.x < 0)
				{
					predictionTime = 0.2f; // If leader is turning right and I am on the left side, predict less
                }
                else if (leader.angularVelocity.y < -0.5f && myPositionInTheFormation.x > 0)
				{
					predictionTime = 0.2f; // If leader is turning left and I am on the right side, predict less
                }
				
				Vector3 leaderPredictedPosition = PredictPosition(leader.Transform, leader.velocity.magnitude, leader.angularVelocity.y * Mathf.Rad2Deg, predictionTime);
                
                targetPosition += leaderPredictedPosition;
				TurnTowardsPosition(targetPosition);
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="transform"></param>
			/// <param name="forwardSpeed"></param>
			/// <param name="angularSpeedY"></param>
			/// <param name="predictionTime">How far in the future to predict</param>
			/// <returns></returns>
            Vector3 PredictPosition(IRelativePositionProvider transform, float forwardSpeed, float angularSpeedY, float predictionTime)
            {
				Vector3 currentPosition = Vector3.zero;

				forwardSpeed *= predictionTime;
                angularSpeedY *= predictionTime;

                Vector3 halfWayForward = transform.forward * forwardSpeed * 0.5f;

				Debug.DrawRay(transform.position, halfWayForward, Color.blue);

                Vector3 halfWayTurned = Quaternion.AngleAxis(angularSpeedY, Vector3.up) * halfWayForward;

				Debug.DrawRay(transform.position + halfWayForward, halfWayTurned, Color.red);

                Vector3 predictedPosition = currentPosition + halfWayForward + halfWayTurned;

                return predictedPosition;
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
