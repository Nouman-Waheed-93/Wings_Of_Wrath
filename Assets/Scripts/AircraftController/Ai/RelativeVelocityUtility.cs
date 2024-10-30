using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AircraftController
{
    public static class RelativeVelocityUtility
    {
        public static float CalculateClosureSpeed(Vector3 positionA, Vector3 positionB, Vector3 RelativeVelocity)
        {
            Vector3 separationDirection = (positionB - positionA).normalized;
            float ClosureSpeed = Vector3.Dot(RelativeVelocity, separationDirection);
            return ClosureSpeed;
        }

        public static float GetDistanceToReachSpeed(float initialSpeed, float finalSpeed, float acceleration)
        {
            float finalSpeedSqr = finalSpeed * finalSpeed;
            float initialSpeedSqr = initialSpeed * initialSpeed;

            float distance = (finalSpeedSqr - initialSpeedSqr)/(2 * acceleration);
            if(distance < 0)
            {
                return Mathf.Infinity;
            }
            return distance;
        }
    }
}
