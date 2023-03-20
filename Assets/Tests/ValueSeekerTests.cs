using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Utilities;

public class ValueSeekerTests
{
    [Test]
    public void ValueIsReachedExactly()
    {
        TargetValueSeeker seeker = new TargetValueSeeker(1);
        seeker.Target = 5;
        seeker.Seek(5);
        Assert.AreEqual(seeker.Target, seeker.CurrValue, "reachSpeed 1 and simulationDeltaTime exactly target");
        seeker.Target = -5;
        seeker.Seek(10);
        Assert.AreEqual(seeker.Target, seeker.CurrValue, "reachSpeed 1 and simulationDeltaTime 10");
    }

    [Test]
    public void ValueIsReachedExactlyWhenDeltaTimeIsGreaterThanTarget()
    {
        TargetValueSeeker seeker = new TargetValueSeeker(10);
        seeker.Target = 5;
        seeker.Seek(1);
        Assert.AreEqual(seeker.Target, seeker.CurrValue);
    }

}
