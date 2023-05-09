using UnityEngine;

namespace Locomotion
{
    [CreateAssetMenu(fileName = "AircraftMovementData", menuName = "ScriptableObjects/AircraftMovementData")]
    public class AircraftMovementData : MovementData
    {
        [Tooltip("the speed at which plane will take off.")]
        public float takeOffSpeed;

        [Tooltip("The maximum pitch angle the aircraft can achieve.")]
        public float maxPitchAngle;
        [Tooltip("The speed with which the target pitch would be achieved.")]
        public float pitchSpeed;

        [Tooltip("The maximum roll angle the aircraft can achieve.")]
        public float maxRollAngle;
        [Tooltip("The speed with which the target roll angle would be achieved")]
        public float rollSpeed;
    }
}
