using UnityEngine.EventSystems;
using UnityEngine;

namespace ScreenInputControls
{
    public abstract class HoldableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        protected bool isHeldDown;

        public virtual void OnPointerDown(PointerEventData eventData) 
        {
            isHeldDown = true;
        }

        public virtual void OnPointerUp(PointerEventData eventData) 
        {
            isHeldDown = false;
        }
    }
}
