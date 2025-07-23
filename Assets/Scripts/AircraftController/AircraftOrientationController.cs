using UnityEngine;
using Locomotion;

namespace AircraftController
{
    public class AircraftOrientationController
    {
        public IPitchYaw turnFactor;

        private Transform transform;

        private IAircraftMovementData movementData;

        private float currPitch;

        private float pitchOffset; //angle in degrees
        public float PitchOffset { get => pitchOffset; set => pitchOffset = value; }

        public AircraftOrientationController(IAircraftMovementData movementData, IPitchYaw turnFactorCalculator, Transform transform)
        {
            this.movementData = movementData;
            this.turnFactor = turnFactorCalculator;
            this.transform = transform;
        }

        public void Update(float simulationDeltaTime)
        {
            transform.localEulerAngles = new Vector3(pitchOffset + movementData.maxPitchAngle * currPitch, 0, movementData.maxRollAngle * -turnFactor.TurnFactor);
        }
    }
}
