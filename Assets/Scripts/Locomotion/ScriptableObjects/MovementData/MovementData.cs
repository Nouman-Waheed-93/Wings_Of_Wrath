using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Locomotion
{
    [CreateAssetMenu(fileName = "MovementData", menuName = "ScriptableObjects/MovementData")]
    public class MovementData : ScriptableObject
    {
        public float maxSpeed;
        public float maxAcceleration;
        public float maxTurnSpeed;
    }
}
