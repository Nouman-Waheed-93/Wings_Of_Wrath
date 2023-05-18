using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AircraftController;
using NSubstitute;
using Locomotion;

public class AircraftStateTests
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

    //[UnityTest]
    //public IEnumerator State_Changes_To_InAir_After_Climbing_Above_AirborneAltitude()
    //{
    //    yield return null;
    //}
}
