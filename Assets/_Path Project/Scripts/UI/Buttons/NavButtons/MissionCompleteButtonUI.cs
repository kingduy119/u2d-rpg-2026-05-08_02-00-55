using UnityEngine;

namespace TDGame
{
    public class MissionCompleteButtonUI : ButtonBase
    {
        protected override void HandleClick()
        {
            GamePlayEvent.MissionCompleteClick?.Invoke();
        }
    }
}