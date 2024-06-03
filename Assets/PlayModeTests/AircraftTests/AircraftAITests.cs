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
    public IEnumerator Aircraft_Turns_Towards_The_WayPoint()
    {
        AircraftAIController aircraft = GetNewAIAircraft();
        yield return null;
        //The waypoint was set to (-1000, 100, -1000) when the aircraft was created.

        aircraft.transform.position = Vector3.zero;
        aircraft.transform.rotation = Quaternion.identity;
        aircraft.Update(1);
        float turn = aircraft.GetTurn();
        Assert.That(turn < 0, "Turn towards waypoint incorrect");

        aircraft.transform.position = new Vector3(-1000, 100, 1000);
        aircraft.Update(1);
        turn = aircraft.GetTurn();
        Assert.That(turn != 0);


        // Test case 1
        aircraft.transform.position = Vector3.zero;
        aircraft.transform.rotation = Quaternion.identity;
        aircraft.Update(1);
        turn = aircraft.GetTurn();
        Assert.That(turn < 0, "Turn towards waypoint incorrect from origin.");

        // Test case 2
        aircraft.transform.position = new Vector3(-1000, 100, 1000);
        aircraft.Update(1);
        turn = aircraft.GetTurn();
        Assert.That(turn != 0, "Turn towards waypoint incorrect from different quadrant.");

        // Test case 3
        aircraft.transform.position = new Vector3(-500, 100, 500);
        aircraft.transform.rotation = Quaternion.Euler(0, 45, 0);
        aircraft.Update(1);
        turn = aircraft.GetTurn();
        Assert.That(turn > 0, "Turn towards waypoint incorrect from negative X and positive Z.");

        // Test case 4
        aircraft.transform.position = new Vector3(500, 100, -500);
        aircraft.transform.rotation = Quaternion.Euler(0, -45, 0);
        aircraft.Update(1);
        turn = aircraft.GetTurn();
        Assert.That(turn < 0, "Turn towards waypoint incorrect from positive X and negative Z.");

        // Test case 5
        aircraft.transform.position = new Vector3(2000, 100, 2000);
        aircraft.transform.rotation = Quaternion.identity;
        aircraft.Update(1);
        turn = aircraft.GetTurn();
        Assert.That(turn != 0, "Turn towards waypoint incorrect from far away.");

        // Test case 8
        aircraft.transform.position = new Vector3(0, 100, -1000);
        aircraft.transform.rotation = Quaternion.Euler(0, 180, 0);
        aircraft.Update(1);
        turn = aircraft.GetTurn();
        Assert.That(turn > 0, "Turn towards waypoint incorrect from same Z but different X.");

        // Test case 9
        aircraft.transform.position = new Vector3(-1000, 100, 0);
        aircraft.transform.rotation = Quaternion.Euler(0, 0, 0);
        aircraft.Update(1);
        turn = aircraft.GetTurn();
        Assert.That(turn != 0, "Turn towards waypoint incorrect from same X but different Z.");

        // Test case 9
        aircraft.transform.position = new Vector3(-1000, 100, -2000);
        aircraft.transform.rotation = Quaternion.Euler(0, 0, 0);
        aircraft.Update(1);
        turn = aircraft.GetTurn();
        Assert.That(turn == 0, "Turn towards waypoint incorrect when facing the waypoint.");

    }

    [UnityTest]
    public IEnumerator Aircraft_Maintains_Correct_Speed_In_Formation()
    {
        FormationSystem.Formation formation = Substitute.For<FormationSystem.Formation>();
        formation.spacing = 10;
        formation.GetMemberPosition(0).Returns(Vector3.zero);
        formation.GetMemberPosition(1).Returns(new Vector3(1, 0, 0));
        AircraftAIController leader = GetNewAIAircraft();
        AircraftAIController aiController = GetNewAIAircraft();

        yield return null;
        formation.AddMember(leader.aircraft.formationMember);
        leader.aircraft.formationMember.Formation = formation;
        formation.leader.Returns(leader.aircraft.formationMember);

        formation.AddMember(aiController.aircraft.formationMember);
        aiController.aircraft.formationMember.Formation = formation;
        aiController.aircraft.formationMember.PositionIndex = 1;

        aiController.StateMachine.ChangeState(aiController.stateFollowFormation);

        float desiredSpeed = 0;
        float leaderSpeed = leader.aircraft.MovementHandler.CurrSpeed;
        leader.transform.position = Vector3.zero;
        leader.transform.rotation = Quaternion.identity;
        aiController.transform.position = new Vector3(0, 0, -100);
        aiController.transform.rotation = Quaternion.identity;
        aiController.Update(0);
        leader.Update(0);
        desiredSpeed = aiController.GetDesiredSpeed();
        leaderSpeed = leader.GetDesiredSpeed();
        Assert.That(desiredSpeed > leaderSpeed);

        aiController.transform.position = Vector3.zero;
        aiController.transform.rotation = Quaternion.identity;
        aiController.Update(0);
        leader.Update(0);
        desiredSpeed = aiController.GetDesiredSpeed();
        leaderSpeed = leader.GetDesiredSpeed();
        Assert.That(desiredSpeed == leaderSpeed);

        aiController.transform.position = new Vector3(0, 0, 100);
        aiController.transform.rotation = Quaternion.identity;
        aiController.Update(0);
        leader.Update(0);
        desiredSpeed = aiController.GetDesiredSpeed();
        leaderSpeed = leader.GetDesiredSpeed();
        Assert.That(desiredSpeed < leaderSpeed);
    }

    [UnityTest]
    public IEnumerator Aircraft_Turns_Towards_Correct_Position_In_Formation()
    {
        FormationSystem.Formation formation = Substitute.For<FormationSystem.Formation>();
        formation.spacing = 10;
        formation.GetMemberPosition(0).Returns(Vector3.zero);
        formation.GetMemberPosition(1).Returns(new Vector3(1, 0, 0));
        AircraftAIController leader = GetNewAIAircraft();
        AircraftAIController aiController = GetNewAIAircraft();

        yield return null;
        formation.AddMember(leader.aircraft.formationMember);
        leader.aircraft.formationMember.Formation = formation;
        formation.leader.Returns(leader.aircraft.formationMember);

        formation.AddMember(aiController.aircraft.formationMember);
        aiController.aircraft.formationMember.Formation = formation;
        aiController.aircraft.formationMember.PositionIndex = 1;

        aiController.StateMachine.ChangeState(aiController.stateFollowFormation);

        float turn = 0;
        turn  = CheckTurnInFormation(leader, aiController, null, null, new Vector3(100, 0, -100), null);
        Assert.That(turn < 0, "Turn calculated incorrectly");

        turn  = CheckTurnInFormation(leader, aiController, null, null, new Vector3(-100, 0, -100));
        Assert.That(turn > 0, "Turn calculated incorrectly");

        turn = CheckTurnInFormation(leader, aiController, null, null, new Vector3(0, 0, -10));
        Assert.That(turn > 0, "Turn calculated incorrectly");

        turn = CheckTurnInFormation(leader, aiController, null, null, new Vector3(10, 0, -10));
        Assert.That(turn == 0, "Turn calculated incorrectly");

        turn = CheckTurnInFormation(leader, aiController, null, null, new Vector3(15, 0, -10));
        Assert.That(turn < 0, "Turn calculated incorrectly");
        //Todo 2 ; Check if the desired speed is calculated perfectly.
        //Assert.AreEqual(60, aiController.GetDesiredSpeed());
    }
    
    private float CheckTurnInFormation(AircraftAIController leader, AircraftAIController follower, Vector3? leaderPos = null, Quaternion? leaderRot = null, Vector3? followerPos = null, Quaternion? followerRot = null)
    {
        Vector3 leaderPosition = Vector3.zero;
        if(leaderPos != null)
        {
            leaderPosition = leaderPos.Value;
        }

        Quaternion leaderRotation = Quaternion.identity;
        if (leaderRot != null)
        {
            leaderRotation = leaderRot.Value;
        }

        Vector3 followerPosition = Vector3.zero;
        if (followerPos != null)
        {
            followerPosition = followerPos.Value;
        }

        Quaternion followerRotation = Quaternion.identity;
        if (followerRot != null)
        {
            followerRotation = followerRot.Value;
        }

        leader.transform.position = leaderPosition;
        leader.transform.rotation = leaderRotation;
        follower.transform.position = followerPosition;
        follower.transform.rotation = followerRotation;
        follower.Update(0);
        float turn = follower.GetTurn();
        return turn;
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
