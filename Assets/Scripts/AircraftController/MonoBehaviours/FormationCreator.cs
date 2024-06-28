using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FormationSystem;

namespace AircraftController
{
    public class FormationCreator : MonoBehaviour
    {
        private Formation currentFormation;
        [SerializeField]
        private AircraftMonoBehaviour aircraftPrefab;
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

        private IEnumerator Start()
        {
            currentFormation = new ArrowHead();
            currentFormation.spacing = this.spacing;
            currentFormation.altitudeSpacing = this.altitudeSpacing;
            for (int i = 0; i < count; i++)
            {
                AircraftMonoBehaviour newAircraft = Instantiate(aircraftPrefab, position + currentFormation.GetMemberPosition(i), Quaternion.identity, transform);
                newAircraft.Init(wayPoints, true, 100, 100, null);
                yield return null;
                currentFormation.AddMember(newAircraft.Aircraft);
                newAircraft.Aircraft.Formation = currentFormation;
            }
            yield return null;
        }
    }
}
