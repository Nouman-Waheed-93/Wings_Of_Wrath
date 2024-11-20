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

        /// <summary>
        /// This calculates the max speed that will be achieved to reach the target speed over a distance.
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="initalSpeed"></param>
        /// <param name="finalSpeed"></param>
        /// <param name="acceleration"></param>
        /// <param name="deceleration"></param>
        /// <returns></returns>
        public static float GetMaxSpeedRequiredToSeek(float distance, float initalSpeed, float finalSpeed, float acceleration, float deceleration)
        {
            Debug.Log("Values received for max speed calculation are :");
            Debug.Log($"distance : {distance}, currSpeed : {initalSpeed}, finalSpeed : {finalSpeed}, acceleration : {acceleration}, deceleration : {deceleration}");
            //The equation used is derived from the formula of acceleration.
            float var1 = 2 * acceleration * deceleration * distance;
            float var2 = deceleration * initalSpeed * initalSpeed;
            float var3 = acceleration * finalSpeed * finalSpeed;
            float result = Mathf.Sqrt((var1 + var2 - var3) / (deceleration - acceleration));
            return result;
        }
    }
}
