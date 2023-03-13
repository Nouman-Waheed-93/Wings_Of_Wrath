using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using ScreenInputControls;

public class ThumbDriftTests
{
    Vector2 targetPosition = new Vector2(0, 5);

    [Test]
    public void Thumb_Behind_Target_Gives_Zero()
    {
        float direction = ThumbDriftLogic.CalculateDirection(targetPosition, new Vector2(0, 0), 90);
        Assert.AreEqual(0, direction);
    }

    [Test]
    public void Thumb_At_Right_Of_Target_Gives_Negative_One()
    {
        float direction = ThumbDriftLogic.CalculateDirection(targetPosition, new Vector3(1, 5), 90);
        Assert.AreEqual(-1, direction);
    }

    [Test]
    public void Thumb_At_Left_Of_Target_Gives_Positive_One()
    {
        float direction = ThumbDriftLogic.CalculateDirection(targetPosition, new Vector3(-1, 5), 90);
        Assert.AreEqual(1, direction);
    }

    [Test]
    public void Thumb_Ahead_Target_Gives_Positive_One()
    {
        float direction = ThumbDriftLogic.CalculateDirection(targetPosition, new Vector3(0, 6), 90);
        Assert.AreEqual(1, direction);
    }

    [Test]
    public void Angle_Between_Thumb_N_Target_Exceeds_Limit_On_Right_Side()
    {
        float direction = ThumbDriftLogic.CalculateDirection(targetPosition, new Vector2(0.01f, 6), 90);
        Assert.AreEqual(-1, direction);
    }

    [Test]
    public void Angle_Between_Thumb_N_Target_Exceeds_Limit_On_Left_Side()
    {
        float direction = ThumbDriftLogic.CalculateDirection(targetPosition, new Vector2(-0.01f, 6), 90);
        Assert.AreEqual(1, direction);
    }

    [Test]
    public void Diagonal_To_Target_Gives_CorrectValue()
    {
        float direction = ThumbDriftLogic.CalculateDirection(targetPosition, new Vector2(1, 4), 90);
        Assert.AreEqual(-0.5f, direction);
        direction = ThumbDriftLogic.CalculateDirection(targetPosition, new Vector2(-1, 4), 90);
        Assert.AreEqual(0.5f, direction);
    }
}
