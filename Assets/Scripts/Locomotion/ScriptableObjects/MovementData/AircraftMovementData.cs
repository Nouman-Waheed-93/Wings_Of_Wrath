using UnityEngine;

namespace Locomotion
{
    [CreateAssetMenu(fileName = "AircraftMovementData", menuName = "ScriptableObjects/AircraftMovementData")]
    public class AircraftMovementData : MovementData
    {
        [Tooltip("The speed at which aircraft will take off.")]
        public float takeOffSpeed;
        [Tooltip("The minimum speed the aircraft can fly at.")]
        public float lowAirSpeed;
        [Tooltip("The normal speed the aircraft will fly at.")]
        public float normalAirSpeed;
        [Tooltip("The top speed the aircraft can fly at.")]
        public float highAirSpeed;

        [Tooltip("Pitch Multiplier")]
        public float maxPitch;

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
