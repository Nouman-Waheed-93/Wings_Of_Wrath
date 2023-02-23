using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Locomotion
{
    [CreateAssetMenu(fileName = "AerodynamicValues", menuName = "ScriptableObjects/AerodynamicSettings")]
    public class AerodynamicMovementSO : ScriptableObject
    {
        public float minSpeed;
        public float maxSpeed;
    }
}
