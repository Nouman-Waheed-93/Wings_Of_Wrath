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
        ClosureSpeedTestCase(new Vector3(0, 0, 0), new Vector3(5, 0, 0), new Vector3(3, 1, 0), new Vector3(5, 0, 0), 0); //Same Direction Same Speed.
        ClosureSpeedTestCase(new Vector3(0, 0, 0), new Vector3(0, 2, 0), new Vector3(0, 100, 0), new Vector3(0, 1, 0), 1);//Same Direction, Follower has greater speed.
        ClosureSpeedTestCase(new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 100, 0), new Vector3(0, 2, 0), -1);//Same Direction, Follower has less speed.
    
        ClosureSpeedTestCase(new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 100, 0), new Vector3(0, -1, 0), 2);//Opposite Direction, facing each other, same speed.
        ClosureSpeedTestCase(new Vector3(0, 0, 0), new Vector3(0, -1, 0), new Vector3(0, 100, 0), new Vector3(0, 1, 0), -2);//Opposite Direction, facing away from each other, same speed.
    }

    private void ClosureSpeedTestCase(Vector3 positionA, Vector3 velocityA, Vector3 positionB, Vector3 velocityB, float expectedResult)
    {
        ITransform TransformA = Substitute.For<ITransform>();
        TransformA.position.Returns(positionA);

        ITransform TransformB = Substitute.For<ITransform>();
        TransformB.position.Returns(positionB);

        Vector3 relativeVelocity = velocityA - velocityB;

        Assert.AreEqual(expectedResult, AircraftController.RelativeVelocityUtility.CalculateClosureSpeed(TransformA.position, TransformB.position, relativeVelocity));
    }

    [Test]
    public void DistanceCanBeCalculatedFromAcceleration()
    {
        Assert.AreEqual(1875f, AircraftController.RelativeVelocityUtility.GetDistanceToReachSpeed(100, 50, -2), "Error while trying to reach a slower speed with deceleration");
        Assert.AreEqual(1875f, AircraftController.RelativeVelocityUtility.GetDistanceToReachSpeed(50, 100, 2), "Error while trying to reach a higher speed with acceleration");
        Assert.AreEqual(Mathf.Infinity, AircraftController.RelativeVelocityUtility.GetDistanceToReachSpeed(100, 50, 2), "Error while trying to reach a slower speed with acceleration");
        Assert.AreEqual(Mathf.Infinity, AircraftController.RelativeVelocityUtility.GetDistanceToReachSpeed(50, 100, -2), "Error while trying to reach a higher speed with deceleration");
    }

    [Test]
    public void MaxSpeedRequiredToSeekIsCorrectlyCalculated()
    {
        Assert.AreEqual(5,
            AircraftController.RelativeVelocityUtility.GetMaxSpeedRequiredToSeek(10, 0, 0, 1, -1));
    }

}
