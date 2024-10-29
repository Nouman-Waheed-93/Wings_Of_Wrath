using Locomotion;
using UnityEngine;
using FormationSystem;

namespace AircraftController
{
    public interface IAircraft
    {
        bool AfterBurnerInput { get; }
        Airstrip AirStripToLandOn { get; set; }
        AircraftMovementHandler MovementHandler { get; }
        AircraftOrientationController OrientationController { get; }
        FinalApproach StateFinalApproach { get; }
        InAir StateInAir { get; }
        Landed StateLanded { get; }
        AircraftStateMachine StateMachine { get; }
        OnGround StateOnGround { get; }
        TakeOff StateTakeOff { get; }
        TouchDown StateTouchDown { get; }
        float Throttle { get; set; }
        Transform Transform { get; }
        float TurnInput { get; }

        IFormationMember formationMember { get; }

        void CalculateAndSetPitch(float targetAltitude, float targetDistance);
        void CalculateAndSetPitch(Vector3 targetPosition);
        bool HasDeviatedFromLine(Vector3 lineStart, Vector3 lineEnd, float acceptableDeviation);
        void SeekSpeed(float targetSpeed);
        float GetRequiredThrottleForSpeed(float targetSpeed);
    }
}