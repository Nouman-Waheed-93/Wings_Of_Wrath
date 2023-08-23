using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace AircraftController
{
    public class AircraftAIController
    {
        private Vector3[] wayPoints;
        private IAircraftController aircraft;
        private int currentIndex;
        private IRelativePositionProvider transform;

        public AircraftAIController(IAircraftController aircraft, IRelativePositionProvider transform, Vector3[] wayPoints)
        {
            this.aircraft = aircraft;
            this.transform = transform;
            this.wayPoints = wayPoints;
        }

        public void Update(float simulationDeltaTime)
        {
            Vector3 currentWP = wayPoints[currentIndex];
            Vector3 relative = transform.GetRelativePosition(currentWP);
            float turnInput = Mathf.Atan2(relative.x, relative.z);
            aircraft.TurnInput  = turnInput;

            if (Vector3.Distance(transform.position, currentWP) < 20)
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
