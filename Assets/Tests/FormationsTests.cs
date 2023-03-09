using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using FormationSystem;

public class FormationsTests
{
    [Test]
    public void Arrow_Head_Formation_Gives_Correct_Position()
    {
        ArrowHead arrowHeadFormation = new ArrowHead();
        Assert.That(arrowHeadFormation.GetMemberPosition(0) == Vector3.zero, "zero index position gives zero Vector");
        Assert.That(arrowHeadFormation.GetMemberPosition(1) == new Vector3(0.707f, 0, -0.707f), "index 1 position gives correct position");
        Assert.That(arrowHeadFormation.GetMemberPosition(2) == new Vector3(-0.707f, 0, -0.707f), "index 2 position gives correct position");
        Assert.That(arrowHeadFormation.GetMemberPosition(3) == new Vector3(0.707f, 0, -0.707f) * 2, "index 3 position gives correct position");
        Assert.That(arrowHeadFormation.GetMemberPosition(4) == new Vector3(-0.707f, 0, -0.707f) * 2, "index 4 position gives correct position");
    }
}
