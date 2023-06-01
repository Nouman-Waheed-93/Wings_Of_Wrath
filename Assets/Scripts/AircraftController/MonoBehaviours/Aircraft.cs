using UnityEngine;
using Locomotion;
using Common;

namespace AircraftController
{
    public class Aircraft : MonoBehaviour, ISpeedProvider
    {
        [SerializeField]
        private AircraftMovementData movementData;

        [SerializeField]
        private Team team;
        public Team Team { get => team; set => team = value; }
        [SerializeField]
        private bool startsInAir;
        [SerializeField]
        private float startAltitude;
        [SerializeField]
        private float startSpeed;

        public float CurrSpeed { get { return aircraftController.MovementHandler.CurrSpeed; } }

        private AircraftController aircraftController;

        private void Awake()
        {
            aircraftController = new AircraftController(movementData, transform, GetComponent<Rigidbody>(), startsInAir, startAltitude, startSpeed);
        }

        private void Update()
        {
            aircraftController.Update(Time.deltaTime);
        }

        public void Turn(float dir)
        {
            aircraftController.TurnInput = dir;
        }

        public void SetAfterBurner(bool isOn)
        {
            aircraftController.AfterBurnerInput = isOn;
        }

        public void PrepareToLand(Airstrip airstrip)
        {
            aircraftController.AirStripToLandOn = airstrip;
        }
    }
}
