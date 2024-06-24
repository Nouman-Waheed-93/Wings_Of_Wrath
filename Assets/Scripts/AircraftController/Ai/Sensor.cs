using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AircraftController
{
    public class Sensor : MonoBehaviour, ISensor
    {
        public bool HasSomethingInFront { get; private set; }

        [SerializeField]
        private float distance;

        private void Update()
        {
            HasSomethingInFront = false;
            if(Physics.Raycast(transform.position, transform.forward, distance))
            {
                HasSomethingInFront = true;
            }
        }

    }
}
