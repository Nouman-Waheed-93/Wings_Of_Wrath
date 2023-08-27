namespace AircraftController
{
    namespace AircraftAI
    {
        public abstract class AIState
        {
            protected AIStateMachine stateMachine;
            protected AircraftAIController aircraftController;

            public AIState(AIStateMachine stateMachine, AircraftAIController aircraftController)
            {
                this.stateMachine = stateMachine;
                this.aircraftController = aircraftController;
            }

            public abstract void Enter();

            public abstract void Update(float simulationDeltaTime);

            public abstract void Exit();
        }
    }
}
