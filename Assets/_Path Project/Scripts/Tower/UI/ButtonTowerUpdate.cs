

namespace TDGame
{
    public class ButtonTowerUpdate : ClickEvent
    {
        public override void RaiseEvent()
        {
            TowerEvent.Log("ButtonTowerUpdate");
        }
    }

}