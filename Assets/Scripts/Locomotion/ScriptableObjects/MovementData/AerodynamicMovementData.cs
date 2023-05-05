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

        [Tooltip("the speed at which plane will take off")]
        public float takeOffSpeed;

        [Tooltip("The factor to reduce the speed when the plane is turning")]
        public float turnSpeedReductionFactor;

        [Tooltip("The minimum speed that can be reached when turning the plane")]
        public float minimumSpeedOnTurn;

        public float maxPitch;
    }
}
