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
        private AircraftOrientationController orientationController;

        private void Awake()
        {
            movementHandler = new AerodynamicMovementHandler(movementData, transform, GetComponent<Rigidbody>());
            orientationController = new AircraftOrientationController(movementHandler, transform.GetChild(0));
            aircraftController = new AircraftController(movementHandler);
        }

        private void Update()
        {
            movementHandler.Update(Time.deltaTime);
            orientationController.Update(Time.deltaTime);
            aircraftController.Update(Time.deltaTime);
        }

        public void Turn(float dir)
        {
            aircraftController.Turn = dir;
        }

        public void SetThrottle(bool isOn)
        {
            aircraftController.IsThrottleOn = isOn;
        }
    }
}
