using NUnit.Framework;
using WeaponSystem;
using NSubstitute;
using Common;

public class RaycastGunTests : WeaponTests
{
    [SetUp]
    public void SetUp()
    {
        weapon = new GunRaycastBased(Substitute.For<ITransform>(), Substitute.For<ITimeProvider>(), 100, 1, 10);
    }

    [Test]
    public void Gun_Instance_Can_Be_Created()
    {
        Assert.IsNotNull(weapon);
    }
}
