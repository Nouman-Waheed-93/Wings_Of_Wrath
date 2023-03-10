using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using FormationSystem;

public static class FormationTestsUtility
{
    public static void Are_Indexes_At_The_Correct_Position(int[] newIndexes, List<FormationMember> formationMembers)
    {
        for (int i = 0; i < newIndexes.Length; i++)
        {
            Assert.AreEqual(i, formationMembers[newIndexes[i]].PositionIndex,
                "Member " + newIndexes[i] + " not at Correct position ");
        }
    }
}
