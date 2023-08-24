using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using FormationSystem;

namespace AircraftController
{
    public class AircraftAIController: IFormationMember
    {
        private Vector3[] wayPoints;
        private IAircraftController aircraft;
        private int currentIndex;
        private IRelativePositionProvider transform;

        public Formation Formation { get; set; }
        public int PositionIndex { get; set; }
        Vector3 IFormationMember.Position { get; set; }
        ITransform IFormationMember.Transform { get => transform; }

        public AircraftAIController(IAircraftController aircraft, IRelativePositionProvider transform, Vector3[] wayPoints)
        {
            this.aircraft = aircraft;
            this.transform = transform;
            this.wayPoints = wayPoints;
        }

        public void Update(float simulationDeltaTime)
        {
            Vector3 targetPosition = wayPoints[currentIndex];
            if(Formation != null)
            {
                targetPosition = Formation.leader.Transform.position + Formation.GetMemberPosition(PositionIndex);
            }
            Vector3 relative = transform.GetRelativePosition(targetPosition);
            float turnInput = Mathf.Atan2(relative.x, relative.z);
            aircraft.TurnInput  = turnInput;

            if (Vector3.Distance(transform.position, targetPosition) < 20)
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
