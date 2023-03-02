namespace AircraftController
{
    public abstract class State
    {
        protected AircraftStateMachine stateMachine;
        protected AircraftController aircraftController;

        public State(AircraftStateMachine stateMachine, AircraftController aircraftController)
        {
            this.stateMachine = stateMachine;
            this.aircraftController = aircraftController;
        }

        public abstract void Enter();

        public abstract void Update(float simulationDeltaTime);

        public abstract void Exit();
    }
}
