using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Locomotion
{
    [RequireComponent(typeof(Rigidbody))]
    public class AerodynamicMovementHandler : MonoBehaviour
    {
        [SerializeField]
        private AerodynamicMovementSO aerodynamicSettings;

        private Rigidbody rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            rigidbody.velocity = transform.forward * 
        }
    }
}