using Assets.Scripts.Interface;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class MyCustomEventTrigger : EventTrigger
    {
        public IOnEventTrigger onEvent = null;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            onEvent.OnPointer(GetComponent<Button>(), Pointer.Down);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            onEvent.OnPointer(GetComponent<Button>(), Pointer.Up);
        }
    }
}
