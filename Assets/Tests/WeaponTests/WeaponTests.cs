using NUnit.Framework;
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

    [Test]
    public void Cannot_Fire_Between_Interval()
    {
        Assert.IsTrue(weapon.Fire(),"Could not First Fire");
        Assert.IsTrue(weapon.ShotInterval > 0, "Shot Interval should be greater than zero");
        weapon.TimeProvider.GetTime().Returns(0);
        Assert.IsFalse(weapon.Fire(), "Could fire before interval");
    }

    [Test]
    public void Can_Fire_After_Interval()
    {
        Assert.IsTrue(weapon.Fire(), "Could not Fire First time");
        Assert.IsTrue(weapon.ShotInterval > 0, "Shot interval should be greater than zero");
        weapon.TimeProvider.GetTime().Returns(weapon.ShotInterval);
        Assert.IsTrue(weapon.Fire(), "Could not fire exactly after interval");
        weapon.TimeProvider.GetTime().Returns(weapon.ShotInterval + 1);
        Assert.IsTrue(weapon.Fire(), "Could not fire 1 second after interval");
    }
}
