using UnityEngine;

namespace Locomotion
{
    public interface IMovementData
    {
        float maxSpeed { get; }
        float maxAcceleration { get; }
        float maxDeceleration { get; }
        float maxBrake { get; }
        float maxTurn { get; }
    }

    [CreateAssetMenu(fileName = "MovementData", menuName = "ScriptableObjects/MovementData")]
    public class MovementData : ScriptableObject, IMovementData
    {
        [field: SerializeField]
        public float maxSpeed { get; private set; } = 50f;
        
        [field: SerializeField]
        public float maxAcceleration { get; private set; } = 3f;

        [field: SerializeField, Tooltip("Normal deceleration on releasing throttle.")]
        public float maxDeceleration { get; private set; } = 1f;
        [field: SerializeField, Tooltip("Max Brake deceleration.")]
        public float maxBrake { get; private set; } = 2f;

        [field: SerializeField]
        public float maxTurn { get; private set; } = 1f;
    }
}
