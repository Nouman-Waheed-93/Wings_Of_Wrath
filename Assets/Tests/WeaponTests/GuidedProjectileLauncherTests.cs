using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using WeaponSystem;
using NSubstitute;

public class GuidedProjectileLauncherTests : WeaponTests
{
    [SetUp]
    public void SetUp()
    {
        weapon = new GuidedProjectileLauncher(Substitute.For<ITransform>(), 100, 1, Substitute.For<IProjectileFactory>());
    }

    [Test]
    public void Gun_Instance_Can_Be_Created()
    {
        Assert.IsNotNull(weapon);
    }
}
