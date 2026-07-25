

using UnityEngine;

namespace TDGame
{
    public class ButtonTowerUpdate : ClickEvent
    {
        public override void RaiseEvent(GameObject go)
        {
            TowerEvent.TowerUpdateSelect();
        }
    }

}