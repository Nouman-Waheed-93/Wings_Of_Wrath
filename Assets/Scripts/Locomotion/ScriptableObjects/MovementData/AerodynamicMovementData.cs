using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Locomotion
{
    [CreateAssetMenu(fileName = "AerodynamicMovementData", menuName = "ScriptableObjects/AerodynamicMovementData")]
    public class AerodynamicMovementData : MovementData
    {
        [Tooltip("the rate of change of direction of velocity")]
        public float velocityDirectionChangeFactor;

        [Tooltip("the speed at which turn can reach full potential")]
        public float turnMaximizationSpeed;
    }
}
