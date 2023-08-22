using UnityEngine;

namespace Common
{
    public class GameTimeProvider : ITimeProvider
    {
        public float GetTime()
        {
            return Time.time;
        }
    }
}
