using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Common
{
    public class PositionIndicator : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        public Transform Target { get => target; set => target = value; }


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

        private void Update()
        {
            Vector3 newPos = cam.WorldToScreenPoint(target.position);
            float angle = 0;
            //GetArrowIndicatorPositionAndAngle(ref newPos, ref angle, screenCentre, screenCentre * 0.9f);
            if (newPos.z < 0)
                newPos *= -1;

            newPos.x = Mathf.Clamp(newPos.x, 0, Screen.width);
            newPos.y = Mathf.Clamp(newPos.y, 0, Screen.height);

            transform.position = newPos;
        }
    }
}
