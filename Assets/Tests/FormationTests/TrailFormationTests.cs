using FormationSystem;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class TrailFormationTests : FormationTests
{
    [Test]
    public void Trail_Formation_Gives_Correct_Position()
    {
        Trail arrowHeadFormation = new Trail();
        Assert.AreEqual(Vector3.zero, arrowHeadFormation.GetMemberPosition(0), "zero index position does not give zero Vector");
        Assert.AreEqual(new Vector3(0, 0, -1), arrowHeadFormation.GetMemberPosition(1), "index 1 position does not give correct position");
        Assert.AreEqual(new Vector3(0, 0, -2), arrowHeadFormation.GetMemberPosition(2), "index 2 position does not give correct position");
        Assert.AreEqual(new Vector3(0, 0, -3), arrowHeadFormation.GetMemberPosition(3), "index 3 position does not give correct position");
        Assert.AreEqual(new Vector3(0, 0, -4), arrowHeadFormation.GetMemberPosition(4), "index 4 position does not give correct position");
    }

    protected override void CreateAFormation(int count)
    {
        formation = new Trail();
        formationMembers = new List<IFormationMember>();
        for (int i = 0; i < count; i++)
        {
            formationMembers.Add(NSubstitute.Substitute.For<IFormationMember>());
            formation.AddMember(formationMembers[i]);
        }
    }
}
