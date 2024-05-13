using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AircraftController.AircraftAI;
using NUnit.Framework;
using NSubstitute;
using AircraftController;
using Locomotion;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using FormationSystem;

public class AircraftAITests
{
    private Vector3[] wayPoints =
        {
            new Vector3(10, 10, 10),
            new Vector3(-10, -10, -10),
            new Vector3(20, 10, 15)};

    private List<GameObject> gameObjectsToDestroyOnTearDown = new List<GameObject>();

    [UnityTest]
    public IEnumerator Aircraft_Follows_Waypoints_When_It_Is_Not_In_Formation()
    {
        AircraftAIController aiController = GetNewAIAircraft();
        yield return null;
        aiController.aircraft.formationMember.Formation = null;
        aiController.Update(0);
        Assert.AreEqual(aiController.stateFollowWaypoints, aiController.StateMachine.currentState);
    }

    [UnityTest]
    public IEnumerator Aircraft_Follows_Waypoints_When_It_Is_The_Leader()
    {
        AircraftAIController aiController = GetNewAIAircraft();
        yield return null;
        aiController.aircraft.formationMember.Formation = Substitute.For<Formation>();
        aiController.Update(0);
        Assert.AreEqual(aiController.stateFollowWaypoints, aiController.StateMachine.currentState);
    }

    [UnityTest]
    public IEnumerator Aircraft_Turns_Towards_Correct_Position_In_Formation()
    {
        FormationSystem.Formation formation = Substitute.For<FormationSystem.Formation>();
        formation.spacing = 10;

        AircraftAIController leader = GetNewAIAircraft();
        AircraftAIController aiController = GetNewAIAircraft();

        yield return null;
        formation.AddMember(leader.aircraft.formationMember);
        leader.aircraft.formationMember.Formation = formation;
        formation.leader.Returns(leader.aircraft.formationMember);

        formation.AddMember(aiController.aircraft.formationMember);
        aiController.aircraft.formationMember.Formation = formation;

        leader.transform.position = Vector3.zero;
        leader.transform.rotation = Quaternion.identity;

        aiController.StateMachine.ChangeState(aiController.stateFollowFormation);
        aiController.transform.position = new Vector3(100, 0, -100);
        aiController.transform.rotation = Quaternion.identity;
 
        aiController.Update(0);
        float turn = aiController.GetTurn();

        Assert.AreEqual(1, turn, "Turn calculated incorrectly");
        //Todo 2 ; Check if the desired speed is calculated perfectly.
        //Assert.AreEqual(0, aiController.GetTurn());
        //Assert.AreEqual(60, aiController.GetDesiredSpeed());
    }

    [UnityTest]
    public IEnumerator Aircraft_Follows_Leader_When_It_Is_Not_The_Leader()
    {
        FormationSystem.Formation formation = Substitute.For<FormationSystem.Formation>();

        AircraftAIController leader = GetNewAIAircraft();
        AircraftAIController aiController = GetNewAIAircraft();
        yield return null;

        formation.AddMember(leader.aircraft.formationMember);
        leader.aircraft.formationMember.Formation = formation;
      
        formation.AddMember(aiController.aircraft.formationMember);
        aiController.aircraft.formationMember.Formation = formation;
        aiController.aircraft.formationMember.PositionIndex = 1;
        aiController.Update(0);
        Assert.AreEqual(aiController.stateFollowFormation, aiController.StateMachine.currentState, "Incorrect State");
    }

    private AircraftAIController GetNewAIAircraft()
    {
        GameObject aircraftGameObject = new GameObject("Aircraft");
        gameObjectsToDestroyOnTearDown.Add(aircraftGameObject);
        GameObject aircraftModelGO = new GameObject("Model");
        aircraftModelGO.transform.parent = aircraftGameObject.transform;
        Rigidbody rigidbody = aircraftGameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;

        Vector3[] wayPoints = {
            new Vector3(-1000, 100, -1000),
            new Vector3(1000, 100, 1000)
        };

        Aircraft aircraft =
            new Aircraft(ScriptableObject.CreateInstance<AircraftMovementData>(), aircraftGameObject.transform, rigidbody, wayPoints);

        return (AircraftAIController)aircraft.AircraftInputController;
    }

    [TearDown]
    public void TearDown()
    {
        for(int i = 0; i < gameObjectsToDestroyOnTearDown.Count; i++)
        {
            GameObject.DestroyImmediate(gameObjectsToDestroyOnTearDown[i]);
        }
        gameObjectsToDestroyOnTearDown.Clear();
    }
}
