using NUnit.Framework;
using HealthSystem;

public class HealthTests
{
    [Test]
    public void Health_Reduces_Correctly()
    {
        Health health = new Health(100);
        health.ReduceHealth(35f);
        Assert.AreEqual(65f, health.CurrHealth, "Health Reduction not working as expected.");
        health.ReduceHealth(4.5f);
        Assert.AreEqual(60.5f, health.CurrHealth, "Health Reduction not working as expected");
        health.ReduceHealth(100f);
        Assert.AreEqual(0, health.CurrHealth, "Health Should not go below zero");
    }

    [Test]
    public void Health_Regains_Correctly()
    {
        Health health = new Health(100, 20f);
        health.ReplenishHealth(30f);
        Assert.AreEqual(50f, health.CurrHealth, "Health improves correctly");
        health.ReplenishHealth(0.5f);
        Assert.AreEqual(50.5f, health.CurrHealth, "Health improves correctly");
        health.ReplenishHealth(100);
        Assert.AreEqual(100, health.CurrHealth, "Health should not go above max health");
    }
}
