using Common;
using NUnit.Framework;
using UnityEngine;
using Utilities;
using NSubstitute;
using Assert = UnityEngine.Assertions.Assert;

public class RelativeVelocityUtilityTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void ClosureSpeedCanBeCalculated()
    {
        TestCase(new Vector3(0, 0, 0), new Vector3(5, 0, 0), new Vector3(3, 1, 0), new Vector3(5, 0, 0), 0); //Same Direction Same Speed.
        TestCase(new Vector3(0, 0, 0), new Vector3(0, 2, 0), new Vector3(0, 100, 0), new Vector3(0, 1, 0), 1);//Same Direction, Follower has greater speed.
        TestCase(new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 100, 0), new Vector3(0, 2, 0), -1);//Same Direction, Follower has less speed.
    
        TestCase(new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 100, 0), new Vector3(0, -1, 0), 2);//Opposite Direction, facing each other, same speed.
        TestCase(new Vector3(0, 0, 0), new Vector3(0, -1, 0), new Vector3(0, 100, 0), new Vector3(0, 1, 0), -2);//Opposite Direction, facing away from each other, same speed.
    }

    private void TestCase(Vector3 positionA, Vector3 velocityA, Vector3 positionB, Vector3 velocityB, float expectedResult)
    {
        ITransform TransformA = Substitute.For<ITransform>();
        TransformA.position.Returns(positionA);

        ITransform TransformB = Substitute.For<ITransform>();
        TransformB.position.Returns(positionB);

        Vector3 relativeVelocity = velocityA - velocityB;

        Assert.AreEqual(expectedResult, AircraftController.RelativeVelocityUtility.CalculateClosureSpeed(TransformA, TransformB, relativeVelocity));
    }

}
