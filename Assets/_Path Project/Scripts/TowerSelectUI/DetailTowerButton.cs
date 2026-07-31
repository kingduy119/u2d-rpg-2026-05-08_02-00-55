
using UnityEngine;

namespace TDGame
{
    public class DetailTowerButton : MonoBehaviour,
    IClickTrigger
    {
        public void RaiseEvent(GameObject go = null)
        {
            // TowerEvent.OnAcceptBuild?.Invoke();
            TowerEvent.OnDetailTower?.Invoke();
        }
    }
}