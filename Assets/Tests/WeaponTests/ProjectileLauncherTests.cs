using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponSystem;
using NSubstitute;
using NUnit.Framework;

public class ProjectileLauncherTests : WeaponTests
{
    [SetUp]
    public void SetUp()
    {
        weapon = new ProjectileLauncher(Substitute.For<ITransform>(), 100, 1, Substitute.For<IProjectileFactory>());
    }

    [Test]
    public void ProjectileLauncher_Instance_Can_Be_Created()
    {
        Assert.IsNotNull(weapon);
    }
}
