using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AircraftController
{
    public class LeaderTurnMonitor
    {
        //-1 indicates that the leader is turning away
        //1 indicates that the leader is turning towards the aircraft
        private float lastTurn;

        public bool IsTurningInwards(float currTurn)
        {
            bool isTurningInwards = false;
            if (lastTurn < currTurn)
            {
                isTurningInwards = true;
            }
            lastTurn = currTurn;
            return isTurningInwards;
        }
    }
}
