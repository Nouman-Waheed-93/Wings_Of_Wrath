using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using WeaponSystem;
using NSubstitute;

public class RaycastGunTests : WeaponTests
{
    [SetUp]
    public void SetUp()
    {
        weapon = new GunRaycastBased(Substitute.For<ITransform>(), 100, 1, 10);
    }

    [Test]
    public void Gun_Instance_Can_Be_Created()
    {
        Assert.IsNotNull(weapon);
    }
}
