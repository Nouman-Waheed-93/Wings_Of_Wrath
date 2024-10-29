using UnityEngine;

namespace Locomotion
{
    [CreateAssetMenu(fileName = "MovementData", menuName = "ScriptableObjects/MovementData")]
    public class MovementData : ScriptableObject
    {
        public float maxSpeed = 50f;
        public float maxAcceleration = 3f;

        [Tooltip("Normal deceleration on releasing throttle.")]
        public float maxDeceleration = 1f;
        [Tooltip("Max Brake deceleration.")]
        public float maxBrake = 2f;

        public float maxTurn = 1f;
    }
}
