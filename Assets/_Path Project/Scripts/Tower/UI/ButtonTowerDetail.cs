

namespace TDGame
{
    public class ButtonTowerDetail : ClickEvent
    {
        public override void RaiseEvent()
        {
            TowerEvent.Log("ButtonTowerUpdate");
        }
    }

}