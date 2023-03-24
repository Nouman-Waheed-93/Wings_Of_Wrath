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

        private float pitchOffset; //angle in degrees
        public float PitchOffset { get => pitchOffset; }

        public AircraftOrientationController(IPitchYaw turnFactorCalculator, Transform transform)
        {
            this.turnFactor = turnFactorCalculator;
            this.transform = transform;
        }

        public void Update(float simulationDeltaTime)
        {
            transform.localEulerAngles = new Vector3(pitchOffset + 30f * currPitch, 0, 90f * -turnFactor.TurnFactor);
        }
    }
}
