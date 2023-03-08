using UnityEngine;

namespace ScreenInputControls
{
    public static class ThumbDriftLogic
    {
        public static float CalculateDirection(Vector3 rightDirection, Vector3 targetPosition,  Vector3 thumbPosition)
        {
            return Vector3.Dot(rightDirection.normalized, (targetPosition - thumbPosition).normalized);
        }
    }
}