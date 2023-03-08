﻿using System.Collections;
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

        public UnityEvent<float> onDirectionChange;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
        }

        void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
        {
            if (!isHeldDown)
                return;

            Vector2 targetScreenPosition = Camera.main.WorldToScreenPoint(target.position);
            float direction = ThumbDriftLogic.CalculateDirection(target.right, targetScreenPosition, eventData.position); //Vector3.Dot(target.right, (targetScreenPosition - eventData.position).normalized);

            onDirectionChange.Invoke(direction);
        }
    }
}