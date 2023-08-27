namespace AircraftController
{
    public abstract class AircraftState
    {
        protected AircraftStateMachine stateMachine;
        protected Aircraft aircraftController;

        public AircraftState(AircraftStateMachine stateMachine, Aircraft aircraftController)
        {
            this.stateMachine = stateMachine;
            this.aircraftController = aircraftController;
        }

        public abstract void Enter();

        public abstract void Update(float simulationDeltaTime);

        public abstract void Exit();
    }
}
