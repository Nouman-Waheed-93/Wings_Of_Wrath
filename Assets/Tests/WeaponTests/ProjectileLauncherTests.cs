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
        Transform barrel = new GameObject().transform;
        weapon = new ProjectileLauncher(barrel, 100, 1, Substitute.For<IProjectileFactory>());
    }

    [Test]
    public void ProjectileLauncher_Instance_Can_Be_Created()
    {
        Assert.IsNotNull(weapon);
    }
}
