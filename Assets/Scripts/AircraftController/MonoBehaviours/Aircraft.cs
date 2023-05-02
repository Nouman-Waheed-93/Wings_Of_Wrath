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

        private void Awake()
        {
            aircraftController = new AircraftController(movementData, transform, GetComponent<Rigidbody>());
        }

        private void Update()
        {
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
            aircraftController.AirStripToLandOn = airstrip;
        }
    }
}
