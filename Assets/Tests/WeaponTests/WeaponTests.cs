using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;
using WeaponSystem;

public abstract class WeaponTests
{
    protected Weapon weapon;

    [Test]
    public void Can_Fire_Weapon()
    {
        Assert.IsTrue(weapon.Fire());
    }

    [Test]
    public void Firing_Weapon_Lessens_Bullets()
    {
        weapon.Fire();
        Assert.AreEqual(99, weapon.RemainingAmmo);
    }
}
