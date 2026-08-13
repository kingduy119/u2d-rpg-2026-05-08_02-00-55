using UnityEngine;

namespace TDGame
{
    public class MissionCompleteButtonUI : ButtonBase
    {
        protected override void HandleClick()
        {
            // GameEvent.NavigateTo?.Invoke("TD_MainMenu");
            GamePlayEvent.MissionCompleteClick?.Invoke();
        }
    }
}