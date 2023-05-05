using UnityEngine;

namespace Locomotion
{
    [CreateAssetMenu(fileName = "AerodynamicMovementData", menuName = "ScriptableObjects/AerodynamicMovementData")]
    public class AerodynamicMovementData : MovementData
    {
        [Tooltip("the speed at which plane will take off")]
        public float takeOffSpeed;

        [Tooltip("The maximum pitch angle the aircraft can achieve")]
        public float maxPitch;
    }
}
