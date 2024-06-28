namespace AircraftController
{
    public class AircraftStateMachine
    {
        public AircraftState currentState { get; private set; }

        public void Initialize(AircraftState initState)
        {
            currentState = initState;
            currentState.Enter();
        }

        public void ChangeState(AircraftState newState)
        {
            currentState?.Exit();

            currentState = newState;
            currentState.Enter();
        }
    }
}
