using UnityEngine;
using Locomotion;
using Common;
using UnityEditor;
using Zenject;
using FormationSystem;

namespace AircraftController
{
    public class AircraftMonoBehaviour : MonoBehaviour, ISpeedProvider
    {
        private Team team;
        public Team Team { get => team; set => team = value; }
        
        [SerializeField]
        private Sensor[] sensors;

        public float CurrSpeed { get { return aircraft.MovementHandler.CurrSpeed; } }

        private IAircraft aircraft;
        public IAircraft Aircraft { get => aircraft; }

        private IFormationMember formationMember;
        public IFormationMember FormationMember { get => formationMember; }

        private IAircraftController aircraftController;
        public IAircraftController AircraftController { get => aircraftController; }

        //private void Start()
        //{
        //    aircraft.SetSensors(sensors);
        //}

        private void OnDrawGizmos()
        {
            if(Application.isPlaying)
                Handles.Label(transform.position, CurrSpeed.ToString());
        }

        [Inject]
        public void Init(IAircraft aircraft, IFormationMember formationMember, IAircraftController controller, Team team)
        {
            this.team = team;
         
            this.aircraft = aircraft;
            this.formationMember = formationMember;
            this.aircraftController = controller;
        }

        public void PrepareToLand(Airstrip airstrip)
        {
            aircraft.AirStripToLandOn = airstrip;
        }

        public class Factory : PlaceholderFactory<AircraftMonoBehaviour>
        {
        }
    }
}
