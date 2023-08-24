using FormationSystem;
using NUnit.Framework;
using System.Collections.Generic;

public abstract class FormationTests
{
    protected Formation formation;
    protected List<IFormationMember> formationMembers;

    [Test]
    public void Member_Cannot_Be_Added_Twice()
    {
        CreateAFormation(0);
        IFormationMember member = NSubstitute.Substitute.For<IFormationMember>();
        formation.AddMember(member);
        formation.AddMember(member);
        Assert.AreEqual(1, formation.MemberCount);
    }

    [Test]
    public void Adding_Member_Twice_Does_Not_Change_Member_Position_Index()
    {
        Formation formation = new Trail();
        IFormationMember member = NSubstitute.Substitute.For<IFormationMember>();
        formation.AddMember(member);
        formation.AddMember(member);
        Assert.AreEqual(member.PositionIndex, 0);
    }

    [Test]
    public virtual void Members_Get_Correct_PositionIndex_After_Adding_In_Formation()
    {
        CreateAFormation(5);
        for (int i = 0; i < formationMembers.Count; i++)
        {
            Assert.AreEqual(i, formationMembers[i].PositionIndex, "Incorrect position index for index number " + i);
        }
    }

    [Test]
    public virtual void Members_Shuffle_Positions_Correctly_After_A_Member_Is_Removed()
    {
        CreateAFormation(10);

        formation.RemoveMember(formationMembers[1]);
        int[] indexesAfterRemovingAt1 = { 0, 2, 3, 4, 5, 6, 7, 8, 9 };
        FormationTestsUtility.Are_Indexes_At_The_Correct_Position(indexesAfterRemovingAt1, formationMembers);
        formation.RemoveMember(formationMembers[9]);
        int[] indexesAfterRemovingAt9 = { 0, 2, 3, 4, 5, 6, 7, 8 };
        FormationTestsUtility.Are_Indexes_At_The_Correct_Position(indexesAfterRemovingAt9, formationMembers);
    }

    [Test]
    public virtual void Removing_The_leader_Refills_The_Spot_Correctly()
    {
        CreateAFormation(6);
        Assert.AreEqual(formationMembers[0], formation.leader);
        formation.RemoveMember(formationMembers[0]);
        int[] newIndexes = { 1, 2, 3, 4, 5 };
        Assert.AreEqual(formationMembers[1], formation.leader);
        FormationTestsUtility.Are_Indexes_At_The_Correct_Position(newIndexes, formationMembers);
    }

    protected abstract void CreateAFormation(int count);
}
