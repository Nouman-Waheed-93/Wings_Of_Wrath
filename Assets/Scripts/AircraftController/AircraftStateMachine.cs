using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AircraftController
{
    public class AircraftStateMachine
    {
        public State currentState { get; private set; }

        public void Initialize(State initState)
        {
            currentState = initState;
        }

        public void ChangeState(State newState)
        {
            currentState?.Exit();

            currentState = newState;
            currentState.Enter();
        }
    }
}
