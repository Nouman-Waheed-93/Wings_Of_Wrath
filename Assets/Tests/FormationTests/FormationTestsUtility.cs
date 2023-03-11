using FormationSystem;
using NUnit.Framework;
using System.Collections.Generic;

public static class FormationTestsUtility
{
    public static void Are_Indexes_At_The_Correct_Position(int[] newIndexes, List<IFormationMember> formationMembers)
    {
        for (int i = 0; i < newIndexes.Length; i++)
        {
            Assert.AreEqual(i, formationMembers[newIndexes[i]].PositionIndex,
                "Member " + newIndexes[i] + " not at Correct position ");
        }
    }
}
