using UnityEngine;
using Locomotion;
using Common;

namespace AircraftController
{
    public class AircraftMonoBehaviour : MonoBehaviour, ISpeedProvider
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
        [SerializeField]
        private Transform[] wayPoints;

        public float CurrSpeed { get { return aircraft.MovementHandler.CurrSpeed; } }

        private Aircraft aircraft;
        public Aircraft Aircraft { get => aircraft; }

        private void Start()
        {
            if(aircraft == null)
                aircraft = new Aircraft(movementData, transform, GetComponent<Rigidbody>(), GetWayPointPositions(), null, startsInAir, startAltitude, startSpeed);
        }

        public void Init(Transform[] waypoints, bool startsInAir, float startAltitude, float startSpeed, IAircraftController aircraftController = null)
        {
            this.wayPoints = waypoints;

            if(aircraft == null)
                aircraft = new Aircraft(movementData, transform, GetComponent<Rigidbody>(), GetWayPointPositions(), aircraftController, startsInAir, startAltitude, startSpeed);
        }

        private Vector3[] GetWayPointPositions()
        {
            Vector3[] wayPoints = new Vector3[this.wayPoints.Length];
            for (int i = 0; i < wayPoints.Length; i++)
            {
                wayPoints[i] = this.wayPoints[i].position;
            }
            return wayPoints;
        }

        private void Update()
        {
            aircraft.Update(Time.deltaTime);
        }

        public void PrepareToLand(Airstrip airstrip)
        {
            aircraft.AirStripToLandOn = airstrip;
        }
    }
}
