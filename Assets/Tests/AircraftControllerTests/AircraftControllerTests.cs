using NUnit.Framework;
using UnityEngine;
using AircraftController;
using NSubstitute;
using Locomotion;
using Assert = UnityEngine.Assertions.Assert;

public class AircraftControllerTests
{

    private AircraftController.AircraftController aircraftController;
    private GameObject aircraftGameObject;

    [SetUp]
    public void SetUp()
    {
        aircraftGameObject = new GameObject();
        GameObject aircraftModelGO = new GameObject();
        aircraftModelGO.transform.parent = aircraftGameObject.transform;
        Rigidbody rigidbody = aircraftGameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        
        aircraftController =
            new AircraftController.AircraftController(ScriptableObject.CreateInstance<AircraftMovementData>(), aircraftGameObject.transform, rigidbody);
    }

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
    public void State_Changes_To_TakeOff_After_Speed_Goes_Above_TakeOffSpeed()
    {
        aircraftController.MovementHandler.SetThrottle(1);
        while(aircraftController.MovementHandler.CurrSpeed <= aircraftController.MovementHandler.AerodynamicMovementData.takeOffSpeed)
        {
            aircraftController.Update(1);
        }

        aircraftController.Update(1);

        Assert.AreEqual(aircraftController.StateTakeOff, aircraftController.StateMachine.currentState);
    }

    [Test]
    public void State_Changes_To_InAir_After_Climbing_Above_AirborneAltitude()
    {
        aircraftController.StateMachine.Initialize(aircraftController.StateTakeOff);

        aircraftGameObject.transform.position = new Vector3(0, GlobalAircraftControllerSettings.airborneAltitude, 0);
        aircraftController.Update(1);
        
        Assert.AreEqual(aircraftController.StateInAir, aircraftController.StateMachine.currentState);
    }

    [Test]
    public void Deviation_From_A_Line_Can_Be_Measured()
    {
        aircraftGameObject.transform.position = Vector3.one * 1.5f;
        Assert.IsFalse(aircraftController.HasDeviatedFromLine(Vector3.zero, Vector3.one, 1));


        aircraftGameObject.transform.position = Vector3.one * 1.6f;
        Assert.IsTrue(aircraftController.HasDeviatedFromLine(Vector3.zero, Vector3.one, 1));

        aircraftGameObject.transform.position = new Vector3(0f, 1f, 0f);
        Assert.IsFalse(aircraftController.HasDeviatedFromLine(Vector3.zero, Vector3.one, 1));

        aircraftGameObject.transform.position = new Vector3(-0.2f, 1.2f, 0);
        Assert.IsTrue(aircraftController.HasDeviatedFromLine(Vector3.zero, Vector3.one, 1));

        aircraftGameObject.transform.position = new Vector3(0.4f, 0.9f, 0);
        Assert.IsFalse(aircraftController.HasDeviatedFromLine(Vector3.zero, new Vector3(1, 0, 0), 1));

        aircraftGameObject.transform.position = new Vector3(0.4f, 1.1f, 0);
        Assert.IsTrue(aircraftController.HasDeviatedFromLine(Vector3.zero, new Vector3(1, 0, 0), 1));
        
        aircraftGameObject.transform.position = new Vector3(0.4f, -0.9f, 0);
        Assert.IsFalse(aircraftController.HasDeviatedFromLine(Vector3.zero, new Vector3(1, 0, 0), 1));

        aircraftGameObject.transform.position = new Vector3(0.4f, -1.1f, 0);
        Assert.IsTrue(aircraftController.HasDeviatedFromLine(Vector3.zero, new Vector3(1, 0, 0), 1));

        ////
        aircraftGameObject.transform.position = new Vector3(0.9f, 0.2f, 0);
        Assert.IsFalse(aircraftController.HasDeviatedFromLine(Vector3.zero, new Vector3(0, 1, 0), 1));

        aircraftGameObject.transform.position = new Vector3(1.1f, 0.8f, 0);
        Assert.IsTrue(aircraftController.HasDeviatedFromLine(Vector3.zero, new Vector3(0, 1, 0), 1));

        aircraftGameObject.transform.position = new Vector3(-0.9f, 0.6f, 0);
        Assert.IsFalse(aircraftController.HasDeviatedFromLine(Vector3.zero, new Vector3(0, 1, 0), 1));

        aircraftGameObject.transform.position = new Vector3(-1.1f, 0.2f, 0);
        Assert.IsTrue(aircraftController.HasDeviatedFromLine(Vector3.zero, new Vector3(0, 1, 0), 1));

    }

    [Test]
    public void Aircraft_Can_Reach_Seek_Speed()
    {
        TestSeekSpeed(10, true);
        TestSeekSpeed(30, true);
        TestSeekSpeed(20, false);
        TestSeekSpeed(50, true);
        TestSeekSpeed(0, false);
    }

    private void TestSeekSpeed(float targetSpeed, bool speedIncreasing)
    {
        aircraftController.SeekSpeed(targetSpeed);
        float prevSpeed = aircraftController.MovementHandler.CurrSpeed;
        aircraftController.MovementHandler.Update(1);

        while (speedIncreasing ? prevSpeed < aircraftController.MovementHandler.CurrSpeed : prevSpeed > aircraftController.MovementHandler.CurrSpeed)
        {
            prevSpeed = aircraftController.MovementHandler.CurrSpeed;
            aircraftController.SeekSpeed(targetSpeed);
            aircraftController.MovementHandler.Update(1);
        }

        Assert.AreApproximatelyEqual(targetSpeed, aircraftController.MovementHandler.CurrSpeed, "Speed not achieved.");
    }
}
