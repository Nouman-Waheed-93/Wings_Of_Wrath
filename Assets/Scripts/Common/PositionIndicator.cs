using UnityEngine;

namespace Common
{
    public class PositionIndicator : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        public Transform Target { get => target; set => target = value; }

        [SerializeField]
        private float edgeDistance;
        [SerializeField]
        private Transform player;

        private RectTransform canvas;
        private Camera cam;

        private RectTransform rectTransform;
        private Vector3 screenCentre;
        
        private void Awake()
        {
            rectTransform = (RectTransform)transform;
            cam = Camera.main;
            canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>(); 
            screenCentre = new Vector3(Screen.width, Screen.height, 0) / 2;
        }

        private void LateUpdate()
        {
            Vector3 newPos = cam.WorldToScreenPoint(target.position);
            if (newPos.z < 0)
                newPos *= -1;

            newPos.x = Mathf.Clamp(newPos.x, 0 + edgeDistance, Screen.width - edgeDistance);
            newPos.y = Mathf.Clamp(newPos.y, 0 + edgeDistance, Screen.height - edgeDistance);

            transform.position = newPos;
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            float angle = Vector3.SignedAngle(player.forward, target.forward, Vector3.down);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
