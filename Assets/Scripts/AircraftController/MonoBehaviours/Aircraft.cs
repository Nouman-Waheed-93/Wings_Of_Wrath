using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Locomotion;

namespace AircraftController
{
    public class Aircraft : MonoBehaviour
    {
        [SerializeField]
        private AerodynamicMovementData movementData;

        private AircraftController aircraftController;
        private AerodynamicMovementHandler movementHandler;

        private void Awake()
        {
            movementHandler = new AerodynamicMovementHandler(movementData, transform, GetComponent<Rigidbody>());
            aircraftController = new AircraftController();
            movementHandler.SetThrottle(1);
        }

        private void Update()
        {
            movementHandler.Update(Time.deltaTime);
        }

        public void Turn(float dir)
        {
            movementHandler.Turn(dir);
        }
    }
}
