using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace AircraftController
{
    public class Airstrip : MonoBehaviour
    {
        [SerializeField]
        private Team team;

        [SerializeField]
        private Transform initialApproach;
        public Transform InitialApproach;

        [SerializeField]
        private Transform finalApproach;
        public Transform FinalApproach;

        [SerializeField]
        private Transform touchDownPoint;
        public Transform TouchDownPoint;

        private void OnTriggerEnter(Collider other)
        {
            //initial approach done
            Aircraft landingAircraft = other.GetComponentInParent<Aircraft>();
            if (landingAircraft.Team != team)
                return;

            //if the aircraft does not have LandingIntent. return
            if (Vector3.Dot(initialApproach.forward, landingAircraft.transform.forward) < 0.5f)
                return;

            landingAircraft.PrepareToLand(this);
        }
    }
}
