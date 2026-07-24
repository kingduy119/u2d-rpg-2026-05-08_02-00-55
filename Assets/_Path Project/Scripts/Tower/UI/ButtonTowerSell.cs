

namespace TDGame
{
    public class TowerSellButton : ClickEvent
    {
        public override void RaiseEvent()
        {
            TowerEvent.Log("TowerSellButton");
        }
    }

}