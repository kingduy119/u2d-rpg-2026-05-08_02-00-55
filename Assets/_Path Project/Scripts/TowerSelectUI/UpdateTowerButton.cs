
using UnityEngine;

namespace TDGame
{
    public class UpdateTowerButton : MonoBehaviour,
    IClickTrigger
    {
        public void RaiseEvent(GameObject go = null)
        {
            TowerEvent.OnSelectUpdateTower?.Invoke();
        }
    }
}