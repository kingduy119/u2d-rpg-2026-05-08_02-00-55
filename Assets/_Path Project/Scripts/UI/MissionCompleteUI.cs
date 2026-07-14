using UnityEngine;

namespace TDGame
{
    public class MissionCompleteUI : MonoBehaviour
    {
        public void SendPlayContinue() => GameEvent.SendPlayContinue();
    }
}