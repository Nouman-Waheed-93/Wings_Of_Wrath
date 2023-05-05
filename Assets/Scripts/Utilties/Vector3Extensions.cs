using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class Vector3Extensions
    {
        public static Vector3 FindNearestPointOnLine(Vector3 origin, Vector3 end, Vector3 point)
        {
            var line = (end - origin);
            var len = line.magnitude;
            line.Normalize();

            var v = point - origin;
            var d = Vector3.Dot(v, line);
            d = Mathf.Clamp(d, 0f, len);
            return origin + line * d;
        }
    }
}
