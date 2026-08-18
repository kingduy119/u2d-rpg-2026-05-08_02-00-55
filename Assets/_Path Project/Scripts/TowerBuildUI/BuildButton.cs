
using UnityEngine;

namespace TDGame
{
    public class BuildButton : MonoBehaviour,
    IClickTrigger
    {
        public void RaiseEvent(GameObject go = null)
        {
            TowerEvent.AcceptBuild?.Invoke();
        }
    }
}