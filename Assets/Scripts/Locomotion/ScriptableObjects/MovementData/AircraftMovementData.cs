using UnityEngine;

namespace Locomotion
{
    [CreateAssetMenu(fileName = "AircraftMovementData", menuName = "ScriptableObjects/AircraftMovementData")]
    public class AircraftMovementData : MovementData
    {
        [Tooltip("The speed at which aircraft will take off.")]
        public float takeOffSpeed = 30f;
        [Tooltip("The minimum speed the aircraft can fly at.")]
        public float lowAirSpeed = 30f;
        [Tooltip("The normal speed the aircraft will fly at.")]
        public float normalAirSpeed = 60f;
        [Tooltip("The top speed the aircraft can fly at.")]
        public float highAirSpeed = 80f;

        [Tooltip("Pitch Multiplier")]
        public float maxPitch = 1f;

        [Tooltip("The maximum pitch angle the aircraft can achieve.")]
        public float maxPitchAngle = 45f;
        [Tooltip("The speed with which the target pitch would be achieved.")]
        public float pitchSpeed = 1f;

        [Tooltip("The maximum roll angle the aircraft can achieve.")]
        public float maxRollAngle = 90f;
        [Tooltip("The speed with which the target roll angle would be achieved")]
        public float rollSpeed = 1f;
    }
}
