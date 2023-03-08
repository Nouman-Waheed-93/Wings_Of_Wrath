using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

namespace ScreenInputControls
{
    public abstract class HoldableButton : IPointerDownHandler, IPointerUpHandler
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
