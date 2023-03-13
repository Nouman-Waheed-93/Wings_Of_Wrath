using UnityEngine;

namespace ScreenInputControls
{
    public static class ThumbDriftLogic
    {
        public static float CalculateDirection(Vector2 targetPosition,  Vector2 thumbPosition, float maxAngle = 180f)
        {
            if (maxAngle > 180f)
                maxAngle = 180f;

            Vector3 relative = targetPosition - thumbPosition;
            float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
            return Mathf.Clamp(angle/maxAngle, -1, 1);
        }
    }
}