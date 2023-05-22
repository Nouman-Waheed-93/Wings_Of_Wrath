using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Utilities;
using Assert = UnityEngine.Assertions.Assert;

public class StaticMethodTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void Nearest_Point_On_Extremeties_Of_A_Line_Can_Be_Derived()
    {
        Assert.IsTrue(Vector3.one == Vector3Extensions.FindNearestPointOnLine(Vector3.zero, Vector3.one, new Vector3(2, 2, 2)),
            "Point above Higher End is not calculated right");

        Assert.IsTrue(Vector3.zero == Vector3Extensions.FindNearestPointOnLine(Vector3.zero, Vector3.one, new Vector3(-2, -2, -2)),
            "Point below the lower End is not calculated right");
    }

    [Test]
    public void Nearest_Point_On_A_Line_To_An_Arbitrary_Point_Can_Be_Calculated()
    {
        Assert.IsTrue(new Vector3(0.5f, 0.5f, 0.5f) == Vector3Extensions.FindNearestPointOnLine(Vector3.zero, Vector3.one, new Vector3(0.4f, 0.6f, 0.5f)));

        Assert.IsTrue(new Vector3(0.1f, 0.1f, 0.1f) == Vector3Extensions.FindNearestPointOnLine(Vector3.zero, Vector3.one, new Vector3(0f, 0.2f, 0.1f)));
    }

}
