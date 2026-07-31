
using UnityEngine;

namespace TDGame
{
    public class CancelBuildButton : MonoBehaviour,
    IClickTrigger
    {
        public void RaiseEvent(GameObject go = null)
        {
            TowerEvent.OnCancelBuild?.Invoke();
        }
    }
}