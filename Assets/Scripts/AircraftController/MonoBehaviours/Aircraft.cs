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
        private MovementHandler movementHandler;

        private void Awake()
        {
            movementHandler = new MovementHandler(movementData, transform, GetComponent<Rigidbody>());
            aircraftController = new AircraftController();
            movementHandler.Accelerate(1);
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
