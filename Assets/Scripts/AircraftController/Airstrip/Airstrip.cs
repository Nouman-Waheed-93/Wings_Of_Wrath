using UnityEngine;
using Common;

namespace AircraftController
{
    public class Airstrip : MonoBehaviour
    {
        [SerializeField]
        private Team team;
        public Team Team { get => team; }

        [SerializeField]
        private Transform initialApproach;
        public Transform InitialApproach { get => initialApproach; }

        [SerializeField]
        private Transform finalApproach;
        public Transform FinalApproach { get => finalApproach; }

        [SerializeField]
        private Transform touchDownPoint;
        public Transform TouchDownPoint { get => touchDownPoint; }
    }
}
