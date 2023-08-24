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
        private AircraftAI[] aircraftAI;

        private void Awake()
        {
            currentFormation = new ArrowHead();
            for (int i = 0; i < aircraftAI.Length; i++)
            {
                currentFormation.AddMember(aircraftAI[i].aiController);
            }
        }
    }
}
