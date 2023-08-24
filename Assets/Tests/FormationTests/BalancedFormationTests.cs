using FormationSystem;
using NUnit.Framework;
using System.Collections.Generic;

public abstract class BalancedFormationTests : FormationTests
{
    [Test]
    public override void Members_Get_Correct_PositionIndex_After_Adding_In_Formation()
    {
        CreateAFormation(5);
        for (int i = 0; i < formationMembers.Count; i++)
        {
            Assert.AreEqual(i, formationMembers[i].PositionIndex, "Incorrect position index for index number " + i);
        }
    }

    [Test]
    public override void Members_Shuffle_Positions_Correctly_After_A_Member_Is_Removed()
    {
        CreateAFormation(10);

        formation.RemoveMember(formationMembers[1]);
        int[] indexesAfterRemovingAt1 = { 0, 3, 2, 5, 4, 7, 6, 9, 8 };
        FormationTestsUtility.Are_Indexes_At_The_Correct_Position(indexesAfterRemovingAt1, formationMembers);

        formation.RemoveMember(formationMembers[2]);
        int[] indexesAfterRemovingAt2 = { 0, 3, 4, 5, 6, 7, 8, 9 };
        FormationTestsUtility.Are_Indexes_At_The_Correct_Position(indexesAfterRemovingAt2, formationMembers);

        formation.RemoveMember(formationMembers[6]);
        int[] indexesAfterRemovingAt6 = { 0, 3, 4, 5, 8, 7, 9 };
        FormationTestsUtility.Are_Indexes_At_The_Correct_Position(indexesAfterRemovingAt6, formationMembers);

        formation.RemoveMember(formationMembers[9]);
        int[] indexsAfterRemovingAt9 = { 0, 3, 4, 5, 8, 7 };
        FormationTestsUtility.Are_Indexes_At_The_Correct_Position(indexsAfterRemovingAt9, formationMembers);

        formation.RemoveMember(formationMembers[7]);
        int[] indexesAfterRemovingAt7 = { 0, 3, 4, 5, 8 };
        FormationTestsUtility.Are_Indexes_At_The_Correct_Position(indexesAfterRemovingAt7, formationMembers);

        formation.RemoveMember(formationMembers[8]);
        int[] indexesAfterRemovingAt8 = { 0, 3, 4, 5 };
        FormationTestsUtility.Are_Indexes_At_The_Correct_Position(indexesAfterRemovingAt8, formationMembers);

        formation.RemoveMember(formationMembers[4]);
        int[] indexesAfterRemovingAt4 = { 0, 3, 5 };
        FormationTestsUtility.Are_Indexes_At_The_Correct_Position(indexesAfterRemovingAt4, formationMembers);

        formation.RemoveMember(formationMembers[3]);
        Assert.AreEqual(0, formationMembers[0].PositionIndex, "Member 0 not at correct position");
        Assert.AreEqual(2, formationMembers[5].PositionIndex, "Member 5 not at correct position");

        formation.RemoveMember(formationMembers[0]);
        Assert.AreEqual(0, formationMembers[5].PositionIndex, "Member 5 not at correct position after leader removal");

    }

    [Test]
    public override void Removing_The_leader_Refills_The_Spot_Correctly()
    {
        CreateAFormation(6);
        Assert.AreEqual(formationMembers[0], formation.leader);
        formation.RemoveMember(formationMembers[0]);
        Assert.AreEqual(formationMembers[2], formation.leader);
        int[] newIndexes = { 2, 1, 4, 3, 5 };
        FormationTestsUtility.Are_Indexes_At_The_Correct_Position(newIndexes, formationMembers);
    }
}
