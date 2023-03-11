using FormationSystem;
using NUnit.Framework;
using UnityEngine;

public class ArrowheadFormationTests
{

    [Test]
    public void Arrow_Head_Formation_Gives_Correct_Position()
    {
        ArrowHead arrowHeadFormation = new ArrowHead();
        Assert.AreEqual(Vector3.zero, arrowHeadFormation.GetMemberPosition(0), "zero index position does not give zero Vector");
        Assert.AreEqual(new Vector3(0.707f, 0, -0.707f), arrowHeadFormation.GetMemberPosition(1), "index 1 position does not give correct position");
        Assert.AreEqual(new Vector3(-0.707f, 0, -0.707f), arrowHeadFormation.GetMemberPosition(2), "index 2 position does not give correct position");
        Assert.AreEqual(new Vector3(0.707f, 0, -0.707f) * 2, arrowHeadFormation.GetMemberPosition(3), "index 3 position does not give correct position");
        Assert.AreEqual(new Vector3(-0.707f, 0, -0.707f) * 2, arrowHeadFormation.GetMemberPosition(4), "index 4 position does not give correct position");
    }

}
