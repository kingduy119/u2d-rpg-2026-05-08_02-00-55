
using UnityEngine;

namespace TDGame
{
    public class SellTowerButton : MonoBehaviour,
    IClickTrigger
    {
        public void RaiseEvent(GameObject go = null)
        {
            // TowerEvent.OnAcceptBuild?.Invoke();
            TowerEvent.OnSelectUpdateTower?.Invoke();
        }
    }
}