using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FormationSystem;
using Zenject;
using AircraftController.AircraftAI;

namespace AircraftController
{
    public class FormationCreator : MonoBehaviour
    {
        [SerializeField]
        private int count;
        [SerializeField]
        private Vector3 position;
        [SerializeField]
        private float spacing;
        [SerializeField]
        private float altitudeSpacing;
        [SerializeField]
        private Transform[] wayPoints;

        private Formation currentFormation;
        private AircraftMonoBehaviour.Factory aircraftFactory;

        [Inject]
        private void Init(AircraftMonoBehaviour.Factory aircraftFactory, Formation formation)
        {
            this.aircraftFactory = aircraftFactory;
            this.currentFormation = formation;
        }

        private IEnumerator Start()
        {
            currentFormation.spacing = this.spacing;
            currentFormation.altitudeSpacing = this.altitudeSpacing;
            for (int i = 0; i < count; i++)
            {
                AircraftMonoBehaviour newAircraft = aircraftFactory.Create();
                newAircraft.transform.position = position + currentFormation.GetMemberPositionSpaced(i);
                newAircraft.transform.rotation = Quaternion.identity;
                newAircraft.transform.SetParent(transform);
                yield return null;
                currentFormation.AddMember(newAircraft.FormationMember);
                newAircraft.FormationMember.Formation = currentFormation;
                ((AircraftAIController)newAircraft.AircraftController).SetWaypoints(GetWaypointPositions());
            }
            yield return null;
        }

        private Vector3[] GetWaypointPositions()
        {
            Vector3[] wps = new Vector3[wayPoints.Length];
            for (int i = 0; i < wayPoints.Length; i++)
            {
                wps[i] = wayPoints[i].position;
            }
            return wps;
        }
    }
}
