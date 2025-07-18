using UnityEngine.EventSystems;
using UnityEngine;

namespace ScreenInputControls
{
    public abstract class HoldableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool isHeldDown { get; private set; }

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
