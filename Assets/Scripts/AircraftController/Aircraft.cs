using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Locomotion;

namespace AircraftController
{
    public class Aircraft : MonoBehaviour
    {
        [SerializeField]
        private MovementData movementData;

        private AircraftController aircraftController;

        private void Awake()
        {
            aircraftController = new AircraftController(new MovementHandler(movementData, transform, GetComponent<Rigidbody>()));
        }
    }
}
