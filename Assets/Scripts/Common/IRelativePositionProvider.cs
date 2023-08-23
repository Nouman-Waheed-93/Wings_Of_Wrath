using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public interface IRelativePositionProvider : ITransform
    {
        public Vector3 GetRelativePosition(Vector3 point);
    }
}
