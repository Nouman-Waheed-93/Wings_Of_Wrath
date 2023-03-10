using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using FormationSystem;
using NUnit.Framework;

public class ArrowheadFormationTests
{

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

}
