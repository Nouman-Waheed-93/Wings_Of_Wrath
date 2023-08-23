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
        private float followSmoothTime = 0.25f;
        [SerializeField]
        private float rotationSpeedMultiplier = 1;
        private Vector3 currVelocity;

        private void LateUpdate()
        {
            SetPosition();
            SetRotation();
        }

        private void SetPosition()
        {
            transform.position = Vector3.SmoothDamp(transform.position, 
                target.position + (target.forward * offset.z) + new Vector3(0, offset.y, 0), 
                ref currVelocity, followSmoothTime);
        }

        private void SetRotation()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, target.eulerAngles.y, 0), rotationSpeedMultiplier * Time.deltaTime);
        }
    }
}