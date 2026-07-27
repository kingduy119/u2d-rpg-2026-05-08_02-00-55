

using UnityEngine;

namespace TDGame
{
    // public class TowerSellButton : ClickEvent
    public class ButtonTowerSell : ClickEvent
    {
        public override void RaiseEvent(GameObject go)
        {
            TowerEvent.Log("TowerSellButton.RaiseEvent");
            // TowerEvent.Log("TowerSellButton");
        }
    }

}