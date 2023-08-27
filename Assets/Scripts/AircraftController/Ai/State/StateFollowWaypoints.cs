using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AircraftController
{
    namespace AircraftAI
    {
        public class StateFollowWaypoints : AIState
        {
            private int currentIndex = 0;
            private Vector3[] wayPoints;

            public StateFollowWaypoints(AIStateMachine stateMachine, AircraftAIController aircraftController, Vector3[] wayPoints): base(stateMachine, aircraftController)
            {
                this.wayPoints = wayPoints;
            }

            public override void Enter()
            {
                aircraftController.SetThrottleNormal();
            }

            public override void Exit()
            {
            }

            public override void Update(float simulationDeltaTime)
            {
                aircraftController.TurnTowardsPosition(wayPoints[currentIndex]);

                if (Vector3.Distance(aircraftController.transform.position, wayPoints[currentIndex]) <= GlobalAircraftControllerSettings.wayPointReachedDistance)
                {
                    currentIndex++;
                    if (currentIndex >= wayPoints.Length)
                    {
                        currentIndex = 0;
                    }
                }
            }
        }
    }
}
