using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ScreenInputControls
{
    public class ThumbDriftInput : HoldableButton, IPointerMoveHandler
    {
        [SerializeField]
        private Transform target; //The target would normally be a player.
                                  //Input would be calculated as direction
                                  //from pointer(finger position) on screen to the target

        [Tooltip("the Angle between targetForward and (thumbPosition -> targetPosition) at which input would be at maximum")]
        [SerializeField]
        private float maxAngle;

        public float Direction { get; private set; }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            UpdateThumbInput(eventData.position);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            Direction = 0;
        }

        void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
        {
            if (!isHeldDown)
                return;

            UpdateThumbInput(eventData.position);
        }
        
        private void UpdateThumbInput(Vector2 thumbPosition)
        {
            Vector2 targetScreenPosition = Camera.main.WorldToScreenPoint(target.position);
            Direction = ThumbDriftLogic.CalculateDirection(targetScreenPosition, thumbPosition, maxAngle);
        }
    }
}
