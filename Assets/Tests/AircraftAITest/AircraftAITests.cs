using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AircraftController.AircraftAI;
using NUnit.Framework;
using NSubstitute;
using AircraftController;
using Locomotion;

public class AircraftAITests
{
    private Vector3[] wayPoints =
        {
            new Vector3(10, 10, 10),
            new Vector3(-10, -10, -10),
            new Vector3(20, 10, 15)};

    private GameObject gameObject;

    [Test]
    public void Aircraft_Follows_Waypoints_When_It_Is_Not_In_Formation()
    {
        AircraftAIController aiController = GetNewAIAircraft();
        aiController.aircraft.formationMember.Formation = null;
        aiController.Update(0);
        Assert.AreEqual(aiController.stateFollowWaypoints, aiController.StateMachine.currentState);
    }

    [Test]
    public void Aircraft_Follows_Waypoints_When_It_Is_The_Leader()
    {
        AircraftAIController aiController = GetNewAIAircraft();
        aiController.aircraft.formationMember.Formation = Substitute.For<FormationSystem.Formation>();
        aiController.Update(0);
        Assert.AreEqual(aiController.stateFollowWaypoints, aiController.StateMachine.currentState);
    }

    [Test]
    public void Aircraft_Turns_Towards_Correct_Position_In_Formation()
    {
        FormationSystem.Formation formation = Substitute.For<FormationSystem.Formation>();
        formation.spacing = 10;

        AircraftAIController leader = GetNewAIAircraft();
        AircraftAIController aiController = GetNewAIAircraft();

        formation.AddMember(leader.aircraft.formationMember);
        leader.aircraft.formationMember.Formation = formation;

        formation.AddMember(aiController.aircraft.formationMember);
        aiController.aircraft.formationMember.Formation = formation;
        aiController.aircraft.formationMember.PositionIndex.Returns(1);
        
        leader.transform.position.Returns(Vector3.zero);
        leader.transform.rotation.Returns(Quaternion.identity);
        leader.transform.forward.Returns(new Vector3(0, 0, 1));

        aiController.transform.position.Returns(new Vector3(100, 0, -100));
        aiController.transform.rotation.Returns(Quaternion.identity);
        aiController.transform.forward.Returns(new Vector3(0, 0, 1));

        aiController.Update(0);
        float turn = aiController.GetTurn();

        Assert.AreEqual(1, turn, "Turn calculated incorrectly");
        //Todo 2 ; Check if the desired speed is calculated perfectly.
        //Assert.AreEqual(0, aiController.GetTurn());
        //Assert.AreEqual(60, aiController.GetDesiredSpeed());

    }

    [Test]
    public void Aircraft_Follows_Leader_When_It_Is_Not_The_Leader()
    {
        FormationSystem.Formation formation = Substitute.For<FormationSystem.Formation>();

        AircraftAIController leader = GetNewAIAircraft();
        AircraftAIController aiController = GetNewAIAircraft();

        formation.AddMember(leader.aircraft.formationMember);
        leader.aircraft.formationMember.Formation = formation;

        formation.AddMember(aiController.aircraft.formationMember);
        aiController.aircraft.formationMember.Formation = formation;
        aiController.aircraft.formationMember.PositionIndex.Returns(1);
        aiController.Update(0);
        Assert.AreEqual(aiController.stateFollowFormation, aiController.StateMachine.currentState, "Incorrect State");
    }

    private AircraftAIController GetNewAIAircraft()
    {
        IAircraft aircraft = Substitute.For<IAircraft>();
        gameObject = new GameObject();
        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        AircraftMovementData movementData = ScriptableObject.CreateInstance<AircraftMovementData>();
        AircraftMovementHandler aircraftMovementHandler = new AircraftMovementHandler(movementData, gameObject.transform, rigidbody);
        aircraft.MovementHandler.Returns(aircraftMovementHandler);
        AircraftAIController aiController = new AircraftAIController(aircraft,
            Substitute.For<Common.IRelativePositionProvider>(), wayPoints);
        return aiController;
    }

}
