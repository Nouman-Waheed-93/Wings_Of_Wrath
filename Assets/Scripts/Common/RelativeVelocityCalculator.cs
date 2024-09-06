using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class RelativeVelocityCalculator
    {
        private IRelativePositionProvider TransformA;
        private IRelativePositionProvider TransformB;

        private Vector3 previousPosA;
        private Vector3 previousPosB;

        public Vector3 RelativeVelocity { get; private set; }
        public float ClosureSpeed { get; private set; }

        private float cumTime;

        public RelativeVelocityCalculator(IRelativePositionProvider transformA, IRelativePositionProvider transformB)
        {
            this.TransformA = transformA;
            this.TransformB = transformB;
            previousPosA = transformA.position;
            previousPosB = transformB.position;
        }

        public void Update(float simulationDeltaTime)
        {
            cumTime += simulationDeltaTime;
            if(cumTime < 0.25f)
            {
                return;
            }
            Vector3 velocityA = (TransformA.position - previousPosA) / cumTime;
            Vector3 velocityB = (TransformB.position - previousPosB) / cumTime;

            cumTime = 0;

            RelativeVelocity = velocityB - velocityA;

            Vector3 separationDirection = (TransformB.position - TransformA.position).normalized;

            ClosureSpeed = Vector3.Dot(RelativeVelocity, separationDirection);

            previousPosA = TransformA.position;
            previousPosB = TransformB.position;
        }

    }
}
