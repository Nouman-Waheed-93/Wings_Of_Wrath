using UnityEngine;

namespace Common
{
    public class PositionIndicator : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        public Transform Target { get => target; set => target = value; }

        private Camera cam;

        private void Awake()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            Vector3 newPos = cam.WorldToScreenPoint(target.position);
            
            if (newPos.z < 0)
                newPos *= -1;

            newPos.x = Mathf.Clamp(newPos.x, 0, Screen.width);
            newPos.y = Mathf.Clamp(newPos.y, 0, Screen.height);

            transform.position = newPos;
        }
    }
}
