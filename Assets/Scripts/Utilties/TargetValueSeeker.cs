using UnityEngine;

namespace Utilities
{
    public class TargetValueSeeker
    {
        private float target;
        public float Target { get => target; set => target = value; }

        private float currValue;
        public float CurrValue { get => currValue; }

        private float reachSpeed;
        public float ReachSpeed { set => reachSpeed = value; }

        public TargetValueSeeker(float reachSpeed) 
        {
            this.reachSpeed = reachSpeed;
        }

        public void Seek(float simulationDeltaTime)
        {
            float seekDelta = reachSpeed * simulationDeltaTime;
            float remainingDifference = Mathf.Abs(target - currValue);
            if (remainingDifference < seekDelta)
                seekDelta = remainingDifference;

            if (currValue < target)
                currValue += seekDelta;
            else if (currValue > target)
                currValue -= seekDelta;
        }
    }
}
