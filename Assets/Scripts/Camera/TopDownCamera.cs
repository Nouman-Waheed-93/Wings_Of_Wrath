using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraController
{
    public class TopDownCamera : MonoBehaviour
    {
        [SerializeField]
        private Vector3 offset;
        [SerializeField]
        private Transform target;
        [SerializeField]
        private float followSpeed;

        private void LateUpdate()
        {
            SetPosition();
            SetRotation();
        }

        private void SetPosition()
        {
            transform.position = target.position + (target.forward * offset.z) + new Vector3(0, offset.y, 0);
        }

        private void SetRotation()
        {
            transform.rotation = Quaternion.Euler(0, target.eulerAngles.y, 0);
        }
    }
}