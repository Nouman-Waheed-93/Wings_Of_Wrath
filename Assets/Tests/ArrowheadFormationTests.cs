using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using FormationSystem;
using NUnit.Framework;

public class ArrowheadFormationTests
{
    ArrowHead formation;
    List<FormationMember> formationMembers;

    [Test]
    public void Arrow_Head_Formation_Gives_Correct_Position()
    {
        ArrowHead arrowHeadFormation = new ArrowHead();
        Assert.That(arrowHeadFormation.GetMemberPosition(0) == Vector3.zero, "zero index position does not give zero Vector");
        Assert.That(arrowHeadFormation.GetMemberPosition(1) == new Vector3(0.707f, 0, -0.707f), "index 1 position does not give correct position");
        Assert.That(arrowHeadFormation.GetMemberPosition(2) == new Vector3(-0.707f, 0, -0.707f), "index 2 position does not give correct position");
        Assert.That(arrowHeadFormation.GetMemberPosition(3) == new Vector3(0.707f, 0, -0.707f) * 2, "index 3 position does not give correct position");
        Assert.That(arrowHeadFormation.GetMemberPosition(4) == new Vector3(-0.707f, 0, -0.707f) * 2, "index 4 position does not give correct position");
    }

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
        CheckIndexesAfterRemovingAMember(indexesAfterRemovingAt1);

        formation.RemoveMember(formationMembers[2]);
        int[] indexesAfterRemovingAt2 = { 0, 3, 4, 5, 6, 7, 8, 9 };
        CheckIndexesAfterRemovingAMember(indexesAfterRemovingAt2);

        formation.RemoveMember(formationMembers[6]);
        int[] indexesAfterRemovingAt6 = { 0, 3, 4, 5, 8, 7, 9 };
        CheckIndexesAfterRemovingAMember(indexesAfterRemovingAt6);

        formation.RemoveMember(formationMembers[9]);
        int[] indexsAfterRemovingAt9 = { 0, 3, 4, 5, 8, 7 };
        CheckIndexesAfterRemovingAMember(indexsAfterRemovingAt9);

        formation.RemoveMember(formationMembers[7]);
        int[] indexesAfterRemovingAt7 = { 0, 3, 4, 5, 8 };
        CheckIndexesAfterRemovingAMember(indexesAfterRemovingAt7);

        formation.RemoveMember(formationMembers[8]);
        int[] indexesAfterRemovingAt8 = { 0, 3, 4, 5 };
        CheckIndexesAfterRemovingAMember(indexesAfterRemovingAt8);

        formation.RemoveMember(formationMembers[4]);
        int[] indexesAfterRemovingAt4 = { 0, 3, 5 };
        CheckIndexesAfterRemovingAMember(indexesAfterRemovingAt4);

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
        int[] newIndexes = { 2, 1, 4, 3, 5};
        CheckIndexesAfterRemovingAMember(newIndexes);
    }

    private void CheckIndexesAfterRemovingAMember(int[] newIndexes)
    {
        for (int i = 0; i < newIndexes.Length; i++)
        {
            Assert.AreEqual(i, formationMembers[newIndexes[i]].PositionIndex,
                "Member " + i + " not at Correct position ");
        }
    }

    private void CreateAFormation(int count)
    {
        formation = new ArrowHead();
        formationMembers = new List<FormationMember>();
        for (int i = 0; i < count; i++)
        {
            formationMembers.Add(new FormationMember());
            formation.AddMember(formationMembers[i]);
        }
    }
}
