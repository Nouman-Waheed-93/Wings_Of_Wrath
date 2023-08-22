using NUnit.Framework;
using WeaponSystem;
using NSubstitute;
using Common;

public class GuidedProjectileLauncherTests : WeaponTests
{
    [SetUp]
    public void SetUp()
    {
        weapon = new GuidedProjectileLauncher(Substitute.For<ITransform>(), Substitute.For<ITimeProvider>(), 100, 1, Substitute.For<IProjectileFactory>());
    }

    [Test]
    public void Gun_Instance_Can_Be_Created()
    {
        Assert.IsNotNull(weapon);
    }
}
