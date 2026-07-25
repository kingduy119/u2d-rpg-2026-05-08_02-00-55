
using UnityEngine;

namespace TDGame
{
    public abstract class ClickEvent : MonoBehaviour,
    IClickTrigger
    {
        public virtual void RaiseEvent(GameObject go)
        {
            throw new System.NotImplementedException();
        }
    }
}