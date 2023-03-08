using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using ScreenInputControls;

public class ThumbDriftTests
{
    [Test]
    public void Thumb_Position_Calculates_Correct_Direction()
    {
        Vector3 targetPosition = new Vector3(0, 5, 0);
        float direction = ThumbDriftLogic.CalculateDirection(Vector3.right, targetPosition, new Vector3(0, 0, 0));
        Assert.That(direction == 0, "Direction Is zero when the thumb is right behind the player position");
        direction = ThumbDriftLogic.CalculateDirection(Vector3.right, targetPosition, new Vector3(1, 5, 0));
        Assert.That(direction == -1, "Direction is -1 when the thumb is on the exact right side of the player");
        direction = ThumbDriftLogic.CalculateDirection(Vector3.right, targetPosition, new Vector3(-1, 5, 0));
        Assert.That(direction == 1, "Direction is 1 when the thumb is on the exact left side of the player");
    }

}
