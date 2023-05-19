using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AircraftController;
using NSubstitute;
using Locomotion;

public class AircraftControllerTests
{
    [Test]
    public void AircraftStateTestsSimplePasses()
    {
        AircraftStateMachine stateMachine = new AircraftStateMachine();
        State stateA = Substitute.For<State>(stateMachine, null);
        State stateB = Substitute.For<State>(stateMachine, null);
        stateMachine.Initialize(stateA);
        Assert.AreEqual(stateA, stateMachine.currentState, "Initial state is incorrect.");
        stateMachine.ChangeState(stateB);
        Assert.AreEqual(stateB, stateMachine.currentState, "State does not change correctly.");
    }

    [Test]
    public void State_Changes_To_InAir_After_Climbing_Above_AirborneAltitude()
    {
        GameObject aircraftGO = new GameObject();
        GameObject aircraftModelGO = new GameObject();
        aircraftModelGO.transform.parent = aircraftGO.transform;
        Rigidbody rigidbody = aircraftGO.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        
        AircraftController.AircraftController aircraftController =
            new AircraftController.AircraftController(ScriptableObject.CreateInstance<AircraftMovementData>(), aircraftGO.transform, rigidbody);

        aircraftController.StateMachine.Initialize(aircraftController.StateTakeOff);

        aircraftGO.transform.position = new Vector3(0, GlobalAircraftControllerSettings.airborneAltitude, 0);
        aircraftController.Update(1);
        

        Assert.AreEqual(aircraftController.StateInAir, aircraftController.StateMachine.currentState);
    }

    [Test]
    public void Speed_Does_Not_Go_In_Reverse_With_Brakes()
    {
        GameObject gameObject = new GameObject();
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        AircraftMovementData movementData = ScriptableObject.CreateInstance<AircraftMovementData>();
        AircraftMovementHandler movementHandler = new AircraftMovementHandler(movementData, gameObject.transform, rb);

        movementHandler.SetBrake(1);
        movementHandler.Update(1);
        Assert.AreEqual(0, movementHandler.CurrSpeed, "Speed Going negative with brakes.");
    }

    [Test]
    public void Aircraft_Maintains_Correct_Speed_Depending_Upon_Throttle()
    {
        GameObject gameObject = new GameObject();
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        AircraftMovementData movementData = ScriptableObject.CreateInstance<AircraftMovementData>();
        AircraftMovementHandler movementHandler = new AircraftMovementHandler(movementData, gameObject.transform, rb);

        movementHandler.SetThrottle(0);
        Assert.AreEqual(0, movementHandler.CurrSpeed, "Speed Not zero at 0 throttle.");

        Assert.AreEqual(movementData.maxSpeed * 0.5f, GetSpeedAtThrottle(movementHandler, 0.5f, true), "Speed Not half at half throttle.");

        Assert.AreEqual(movementData.maxSpeed, GetSpeedAtThrottle(movementHandler, 1, true), "Speed Not Max at Max throttle.");

        Assert.AreEqual(movementData.maxSpeed * 0.5f, GetSpeedAtThrottle(movementHandler, 0.5f, false), "Speed Slows down on reducing throttle.");
    }

    private float GetSpeedAtThrottle(MovementHandler movementHandler, float throttle, bool speedIncreasing)
    {
        movementHandler.SetThrottle(throttle);
        float prevSpeed = movementHandler.CurrSpeed;
        movementHandler.Update(1);

        while (speedIncreasing? prevSpeed < movementHandler.CurrSpeed : prevSpeed > movementHandler.CurrSpeed)
        {
            prevSpeed = movementHandler.CurrSpeed;
            movementHandler.Update(1);
        }
        return movementHandler.CurrSpeed;
    }

}
