using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AircraftController
{
    public class AIController : MonoBehaviour
    {
        [SerializeField]
        private Transform[] wayPoints;

        private Aircraft aircraft;
        private int currentIndex;

        private void Start()
        {
            aircraft = GetComponent<Aircraft>();
        }

        private void Update()
        {
            Vector3 currentWP = wayPoints[currentIndex].position;
            Vector3 relative = transform.InverseTransformPoint(currentWP);
            float turnInput = Mathf.Atan2(relative.x, relative.z);
            aircraft.Turn(turnInput);

            if(Vector3.Distance(transform.position, currentWP) < 20)
            {
                currentIndex++;
                if(currentIndex >= wayPoints.Length)
                {
                    currentIndex = 0;
                }
            }
        }
    }
}
