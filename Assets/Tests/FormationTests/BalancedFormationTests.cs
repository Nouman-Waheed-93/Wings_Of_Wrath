using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using FormationSystem;

public class BalancedFormationTests
{
    BalancedFormation formation;
    List<FormationMember> formationMembers;

    [Test]
    public void Members_Get_Correct_PositionIndex_After_Adding_In_Formation()
    {
        CreateAFormation(5);
        for (int i = 0; i < formationMembers.Count; i++)
        {
            Assert.That(formationMembers[i].PositionIndex == i, "Incorrect position index for index number " + i);
        }
    }

    [Test]
    public void Members_Shuffle_Positions_Correctly_After_A_Member_Is_Removed()
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
    public void Removing_The_leader_Shuffles_The_Formation_Correctly()
    {
        CreateAFormation(6);
        formation.RemoveMember(formationMembers[0]);
        int[] newIndexes = { 2, 1, 4, 3, 5 };
        FormationTestsUtility.Are_Indexes_At_The_Correct_Position(newIndexes, formationMembers);
    }

    private void CreateAFormation(int count)
    {
        formation = new BattleSpread();
        formationMembers = new List<FormationMember>();
        for (int i = 0; i < count; i++)
        {
            formationMembers.Add(new FormationMember());
            formation.AddMember(formationMembers[i]);
        }
    }
}
