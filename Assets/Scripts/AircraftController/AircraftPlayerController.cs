using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ScreenInputControls;

namespace AircraftController
{
    public class AircraftPlayerController : IAircraftController, ITickable
    {
        private IAircraft aircraft;

        private bool isAfterBurnerOn;

        private float turnInput;

        public bool IsAfterBurnerOn { get => isAfterBurnerOn; }
        
        public float AltitudeOffset { get => 0; }

        private ThumbDriftInput inputController;

        public AircraftPlayerController(IAircraft aircraft, ThumbDriftInput inputController)
        {
            this.aircraft = aircraft;
            this.inputController = inputController;
        }

        public float GetDesiredSpeed()
        {
            return 80;
        }

        public float GetTurn()
        {
            return turnInput;
        }

        public void Tick()
        {
            Update(Time.deltaTime);
        }

        public void Update(float simulationDeltaTime)
        {
            turnInput = inputController.Direction;
            isAfterBurnerOn = inputController.isHeldDown;
            aircraft.TurnInput = turnInput;
            aircraft.AfterBurnerInput = isAfterBurnerOn;
            aircraft.DesiredSpeed = 80;
        }
    }
}
