using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Locomotion;

namespace AircraftController
{
    public class AircraftOrientationController
    {
        public IPitchYaw turnFactor;

        private Transform transform;

        private float currPitch;

        public AircraftOrientationController(IPitchYaw turnFactorCalculator, Transform transform)
        {
            this.turnFactor = turnFactorCalculator;
            this.transform = transform;
        }

        public void Update(float simulationDeltaTime)
        {
            transform.localEulerAngles = new Vector3(30f * currPitch, 0, 90f * -turnFactor.TurnFactor);
        }

    }
}
