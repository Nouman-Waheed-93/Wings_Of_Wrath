using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AircraftController;
using NSubstitute;
using Locomotion;

public class MovementHandlerTests 
{

    private MovementData movementData;
    private MovementHandler movementHandler;

    [SetUp]
    public void SetUp()
    {
        GameObject gameObject = new GameObject();
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        movementData = ScriptableObject.CreateInstance<MovementData>();
        movementHandler = new MovementHandler(movementData, gameObject.transform, rb);
    }

    [Test]
    public void Speed_Does_Not_Go_In_Reverse_With_Brakes()
    {
        movementHandler.SetBrake(1);
        movementHandler.Update(1);
        Assert.AreEqual(0, movementHandler.CurrSpeed, "Speed Going negative with brakes.");
    }

    [Test]
    public void Aircraft_Maintains_Correct_Speed_Depending_Upon_Throttle()
    {
        Assert.AreEqual(0, GetSpeedAtThrottle(0, true), "Speed Not zero at 0 throttle.");

        Assert.AreEqual(movementData.maxSpeed * 0.5f, GetSpeedAtThrottle(0.5f, true), "Speed Not half at half throttle.");

        Assert.AreEqual(movementData.maxSpeed, GetSpeedAtThrottle(1, true), "Speed Not Max at Max throttle.");

        Assert.AreEqual(movementData.maxSpeed * 0.5f, GetSpeedAtThrottle(0.5f, false), "Speed Slows down on reducing throttle.");
    }

    private float GetSpeedAtThrottle(float throttle, bool speedIncreasing)
    {
        movementHandler.SetThrottle(throttle);
        float prevSpeed = movementHandler.CurrSpeed;
        movementHandler.Update(1);

        while (speedIncreasing ? prevSpeed < movementHandler.CurrSpeed : prevSpeed > movementHandler.CurrSpeed)
        {
            prevSpeed = movementHandler.CurrSpeed;
            movementHandler.Update(1);
        }
        return movementHandler.CurrSpeed;
    }

}
