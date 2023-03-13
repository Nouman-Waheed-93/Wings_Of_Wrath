using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AircraftController
{
    public class AircraftOrientationController
    {
        private Transform transform;

        private float targetPitch;
        public float TargetPitch { set => targetPitch = value; }

        private float targetTurn;
        public float TargetTurn { set => targetTurn = -value; }

        private float currPitch;
        private float currTurn;

        private float pitchReachSpeed;
        private float turnReachSpeed;

        public AircraftOrientationController(Transform transform, float pitchReachSpeed, float turnReachSpeed)
        {
            this.transform = transform;
            this.pitchReachSpeed = pitchReachSpeed;
            this.turnReachSpeed = turnReachSpeed;
        }

        public void Update(float simulationDeltaTime)
        {
            ReachValue(ref currPitch, targetPitch, pitchReachSpeed * simulationDeltaTime);
            ReachValue(ref currTurn, targetTurn, turnReachSpeed * simulationDeltaTime);
            transform.localEulerAngles = new Vector3(30f * currPitch, 0, 90f * currTurn);
        }

        private void ReachValue(ref float currValue, float targetValue, float reachSpeed)
        {
            float difference = Mathf.Abs(targetValue - currValue);
            if (difference < reachSpeed)
                reachSpeed = difference;

            if (currValue < targetValue)
                currValue += reachSpeed;
            else if (currValue > targetValue)
                currValue -= reachSpeed;
        }
    }
}
