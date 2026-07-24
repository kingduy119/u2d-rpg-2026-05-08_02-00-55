
using UnityEngine;

namespace TDGame
{
    public abstract class ClickEvent : MonoBehaviour,
IClickTrigger
    {
        public virtual void RaiseEvent()
        {
            throw new System.NotImplementedException();
        }
    }
}