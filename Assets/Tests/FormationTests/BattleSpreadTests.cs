using FormationSystem;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BattleSpreadTests : BalancedFormationTests
{
    [Test]
    public void BattleSpread_Formation_Gives_Correct_Position()
    {
        BattleSpread battleSpread = new BattleSpread();
        Assert.AreEqual(Vector3.zero, battleSpread.GetMemberPosition(0), "zero index position does not give zero Vector");
        Assert.AreEqual(new Vector3(1, 0, 0), battleSpread.GetMemberPosition(1), "index 1 position does not give correct position");
        Assert.AreEqual(new Vector3(-1, 0, 0), battleSpread.GetMemberPosition(2), "index 2 position does not give correct position");
        Assert.AreEqual(new Vector3(2, 0, 0), battleSpread.GetMemberPosition(3), "index 3 position does not give correct position");
        Assert.AreEqual(new Vector3(-2, 0, 0), battleSpread.GetMemberPosition(4), "index 4 position does not give correct position");
    }

    protected override void CreateAFormation(int count)
    {
        formation = new BattleSpread();
        formationMembers = new List<IFormationMember>();
        for (int i = 0; i < count; i++)
        {
            formationMembers.Add(NSubstitute.Substitute.For<IFormationMember>());
            formation.AddMember(formationMembers[i]);
        }
    }
}
