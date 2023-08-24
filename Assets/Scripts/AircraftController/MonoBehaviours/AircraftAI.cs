using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FormationSystem;
using Common;

namespace AircraftController
{
    public class AircraftAI : MonoBehaviour, IRelativePositionProvider
    {
        [SerializeField]
        private Transform[] wayPoints;

        public AircraftAIController aiController { get; private set; }
        
        private new Transform transform;

        public Vector3 position { get => transform.position; set => transform.position = value; }
        public Quaternion rotation { get => transform.rotation; set => transform.rotation = value; }

        public Vector3 forward => transform.forward;

        public Vector3 GetRelativePosition(Vector3 point)
        {
            return transform.InverseTransformPoint(point);
        }

        private void Awake()
        {
            transform = base.transform;
        }

        private void Start()
        {
            Aircraft aircraft = GetComponent<Aircraft>();
            aiController = new AircraftAIController(aircraft.AircraftController, this, GetWayPointPositions());
        }

        private void Update()
        {
            aiController.Update(Time.deltaTime);
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
    }
}
