using UnityEngine;

namespace Locomotion
{
    public interface IAircraftMovementData : IMovementData
    {
        float takeOffSpeed { get; }
        float lowAirSpeed { get; }
        float normalAirSpeed { get; }
        float highAirSpeed { get; }

        float maxPitch { get; }
        float maxPitchAngle { get; }
        float pitchSpeed { get; }

        float maxRollAngle { get; }
        float rollSpeed { get; }
    }

    [CreateAssetMenu(fileName = "AircraftMovementData", menuName = "ScriptableObjects/AircraftMovementData")]
    public class AircraftMovementData : MovementData, IAircraftMovementData
    {
        [field: SerializeField, Tooltip("The speed at which aircraft will take off.")]
        public float takeOffSpeed { get; private set; } = 30f;
        [field: SerializeField, Tooltip("The minimum speed the aircraft can fly at.")]
        public float lowAirSpeed { get; private set; } = 30f;
        [field: SerializeField, Tooltip("The normal speed the aircraft will fly at.")]
        public float normalAirSpeed { get; private set; } = 60f;
        [field: SerializeField, Tooltip("The top speed the aircraft can fly at.")]
        public float highAirSpeed { get; private set; } = 80f;

        [field: SerializeField, Tooltip("Pitch Multiplier")]
        public float maxPitch { get; private set; } = 1f;

        [field: SerializeField, Tooltip("The maximum pitch angle the aircraft can achieve.")]
        public float maxPitchAngle { get; private set; } = 45f;
        [field: SerializeField, Tooltip("The speed with which the target pitch would be achieved.")]
        public float pitchSpeed { get; private set; } = 1f;

        [field: SerializeField, Tooltip("The maximum roll angle the aircraft can achieve.")]
        public float maxRollAngle { get; private set; } = 90f;
        [field: SerializeField, Tooltip("The speed with which the target roll angle would be achieved")]
        public float rollSpeed { get; private set; } = 1f;
    }
}
