using UnityEngine;

namespace AircraftController
{
    public class InitialApproach : MonoBehaviour
    {
        [SerializeField]
        Airstrip airstrip;
        private void OnTriggerEnter(Collider other)
        {
            Aircraft landingAircraft = other.GetComponentInParent<Aircraft>();
            if (landingAircraft == null)
                return;
            if(landingAircraft.Team != airstrip.Team)
                return;

            //if the aircraft does not have LandingIntent. return
            if (Vector3.Angle(transform.forward, landingAircraft.transform.forward) > GlobalAircraftControllerSettings.maxAngleErrorOnInitialApproach)
                return;

            Debug.Log("Initial Approach done");
            //initial approach done
            landingAircraft.PrepareToLand(airstrip);
        }
    }
}
