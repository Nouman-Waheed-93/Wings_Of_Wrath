using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using WeaponSystem;

public class RaycastGunTests : WeaponTests
{
    [SetUp]
    public void SetUp()
    {
        Transform barrel = new GameObject().transform;
        weapon = new GunRaycastBased(barrel, 100, 1, 10);
    }


    [Test]
    public void Gun_Instance_Can_Be_Created()
    {
        Assert.IsNotNull(weapon);
    }
}
