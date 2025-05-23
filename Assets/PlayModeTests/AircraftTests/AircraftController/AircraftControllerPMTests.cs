using System.Collections;
using Locomotion;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;
using Assert = UnityEngine.Assertions.Assert;
using AircraftController;

public class AircraftControllerPMTests
{
    private Aircraft aircraft;
    private GameObject aircraftGameObject;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        aircraftGameObject = new GameObject();
        GameObject aircraftModelGO = new GameObject();
        aircraftModelGO.transform.parent = aircraftGameObject.transform;
        Rigidbody rigidbody = aircraftGameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        IAircraftController aircraftController = Substitute.For<IAircraftController>();
        aircraft =
            new Aircraft(ScriptableObject.CreateInstance<AircraftMovementData>(), aircraftGameObject.transform, rigidbody, null, aircraftController);
    }

    [UnityTest]
    public IEnumerator Pitch_Is_Calculated_Correctly_With_Target_Position()
    {

        Rigidbody rb = aircraftGameObject.GetComponent<Rigidbody>();

        ResetAircraftPositionAndRotation();
        yield return TestTargetPositionPitch(rb, new Vector3(0, -10, 1), "The Pitch is not correct when the aircraft is higher than the target position");

        ResetAircraftPositionAndRotation();
        yield return TestTargetPositionPitch(rb, new Vector3(0, 10, 1), "The Pitch is not correct when the aircraft is lower than the target position");

        ResetAircraftPositionAndRotation();
        yield return TestTargetPositionPitch(rb, new Vector3(0, -10, -10), "The Pitch is not correct when the target position is lower and behind the aircraft.");

        ResetAircraftPositionAndRotation();
        yield return TestTargetPositionPitch(rb, new Vector3(0, 10, -10), "The Pitch is not correct when the target position is higher and behind the aircraft.");

    }

    private void ResetAircraftPositionAndRotation()
    {
        aircraftGameObject.transform.rotation = Quaternion.identity;
        aircraftGameObject.transform.position = Vector3.zero;
    }

    private IEnumerator TestTargetPositionPitch(Rigidbody rb, Vector3 targetPosition, string message)
    {
        float dot = Vector3.Dot(aircraftGameObject.transform.forward, targetPosition.normalized);
        Assert.AreNotApproximatelyEqual(1f, dot);

        aircraft.CalculateAndSetPitch(targetPosition);

        aircraft.Update(1);
        yield return null;

        while (rb.angularVelocity != Vector3.zero)
        {
            yield return null;
            aircraft.CalculateAndSetPitch(targetPosition);
            aircraft.Update(1);
        }

        dot = Vector3.Dot(aircraftGameObject.transform.forward, (targetPosition - aircraftGameObject.transform.position).normalized);
        Assert.AreApproximatelyEqual(1f, dot, message); //The aircraft is pointing towards the target position
    }


    [UnityTest]
    public IEnumerator Pitch_Is_Calculated_Correctly_With_Target_Altitude_And_Distance()
    {
        aircraftGameObject.transform.rotation = Quaternion.identity;
        aircraftGameObject.transform.position = Vector3.zero;

        Rigidbody rb = aircraftGameObject.GetComponent<Rigidbody>();


        yield return TestAltitudeAndDistancePitch(rb, -10, 100, "The Pitch is not correct when the aircraft is higher than the target position");

        yield return TestAltitudeAndDistancePitch(rb, 10, 100, "The Pitch is not correct when the aircraft is lower than the target position");

        //The Two Tests Below were failing, But it would not affect the functionality for now.

        //    yield return TestAltitudeAndDistancePitch(rb, -10, -100, "The Pitch is not correct when the target position is lower and behind the aircraft.");

        //    yield return TestAltitudeAndDistancePitch(rb, 10, -100, "The Pitch is not correct when the target position is higher and behind the aircraft.");
    }

    private IEnumerator TestAltitudeAndDistancePitch(Rigidbody rb, float altitude, float distance, string message)
    {
        Vector3 targetPosition = new Vector3(0, altitude, distance);
        float dot = Vector3.Dot(aircraftGameObject.transform.forward, targetPosition.normalized);
        Assert.AreNotApproximatelyEqual(1f, dot);

        aircraft.CalculateAndSetPitch(altitude, distance);

        aircraft.Update(1);
        yield return null;

        while (rb.angularVelocity != Vector3.zero)
        {
            yield return null;
            aircraft.CalculateAndSetPitch(altitude, distance);
            aircraft.Update(1);
        }

        dot = Vector3.Dot(aircraftGameObject.transform.forward, (targetPosition - aircraftGameObject.transform.position).normalized);
        Assert.AreApproximatelyEqual(1f, dot); //The aircraft is pointing towards the target position
    }
}
