using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AircraftController
{
    public static class RelativeVelocityUtility
    {
        public static float CalculateClosureSpeed(ITransform TransformA, ITransform TransformB, Vector3 RelativeVelocity)
        {
            Vector3 separationDirection = (TransformB.position - TransformA.position).normalized;
            float ClosureSpeed = Vector3.Dot(RelativeVelocity, separationDirection);
            return ClosureSpeed;
        }
    }
}
