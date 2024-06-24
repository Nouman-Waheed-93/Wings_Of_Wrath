using System.Collections;
using System.Collections.Generic;
using AircraftController;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class LeaderTurnMonitorTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void LeaderTurnMonitorTestSimplePasses()
    {
        LeaderTurnMonitor turnMonitor = new LeaderTurnMonitor();
        Assert.IsTrue(turnMonitor.IsTurningInwards(1));
        Assert.IsTrue(turnMonitor.IsTurningInwards(2));
        Assert.IsFalse(turnMonitor.IsTurningInwards(1));
    }
}
