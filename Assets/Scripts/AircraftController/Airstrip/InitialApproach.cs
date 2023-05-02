using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AircraftController
{
    public class InitialApproach : MonoBehaviour
    {
        [SerializeField]
        Airstrip airstrip;
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("here running");
            //initial approach done
            Aircraft landingAircraft = other.GetComponentInParent<Aircraft>();
            if (landingAircraft.Team != airstrip.Team)
                return;

            //if the aircraft does not have LandingIntent. return
            if (Vector3.Dot(transform.forward, landingAircraft.transform.forward) < 0.5f)
                return;

            landingAircraft.PrepareToLand(airstrip);
        }
    }
}
