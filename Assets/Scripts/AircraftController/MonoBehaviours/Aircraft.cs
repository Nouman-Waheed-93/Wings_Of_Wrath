using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Locomotion;
using Common;

namespace AircraftController
{
    public class Aircraft : MonoBehaviour
    {
        [SerializeField]
        private AerodynamicMovementData movementData;

        [SerializeField]
        private Team team;
        public Team Team { get => team; set => team = value; }

        private AircraftController aircraftController;
        private AerodynamicMovementHandler movementHandler;
        private AircraftOrientationController orientationController;
        private Airstrip airstripToLandAt;

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

        public void SetThrottle(float throttle)
        {
            aircraftController.Throttle = throttle;
        }

        public void PrepareToLand(Airstrip airstrip)
        {
            airstripToLandAt = airstrip;
        }
    }
}
