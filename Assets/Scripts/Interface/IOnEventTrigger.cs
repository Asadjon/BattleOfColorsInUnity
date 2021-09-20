using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Interface
{
    internal interface IOnEventTrigger
    {
        void OnPointer(Button button, Pointer pointer);
    };

    public enum Pointer { Down, Up}
}
