
using System;

namespace TDGame
{
    public static class InGameEvent
    {
        public static Action OnPathwayRaise;
        public static Action OnSpawnerRaise;

        public static Action OnStartWave;
        public static Action OnEndWave;
        public static Action OnPauseGame;
        public static Action OnGameSpeedChanged;

        public static Action<bool> OnActiveStartWaveButton;

        public static Action OnMissionComplete;
    }
}

