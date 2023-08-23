using NUnit.Framework;
using HealthSystem;

public class DamageablePartTest
{
    [Test]
    public void Damageable_Part_Can_Be_Created()
    {
        DamageablePartController damageablePart = new DamageablePartController(new Health(100), 1);
        Assert.IsNotNull(damageablePart);
    }

    [Test]
    public void Damageable_Gets_Correct_Damage()
    {
        Health health = new Health(100);
        DamageablePartController damageablePart = new DamageablePartController(health, 1);
        damageablePart.Damage(3);
        Assert.AreEqual(97, health.CurrHealth);
        damageablePart = new DamageablePartController(health, 7);
        damageablePart.Damage(1);
        Assert.AreEqual(90, health.CurrHealth);
    }
}
