using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AircraftController
{
    namespace AircraftAI
    {
        public class AIStateMachine
        {
            public AIState currentState { get; private set; }

            public void Initialize(AIState initState)
            {
                currentState = initState;
                currentState.Enter();
            }

            public void ChangeState(AIState newState)
            {
                currentState?.Exit();

                currentState = newState;
                currentState.Enter();
            }
        }
    }
}
