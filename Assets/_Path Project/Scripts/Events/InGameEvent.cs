
using System;

namespace TDGame
{
    public static class InGameEvent
    {
        public static Action PathwayStart;
        public static Action PathwayEnd;
        public static Action SpawnerStart;
        public static Action SpawnerEnd;
        public static Action WaveCompleted;
        public static Action<LevelSO> OnLevelLoaded;

        public static Action<int> StartWave;
        public static Action OnEndWave;
        public static Action OnPauseGame;
        public static Action OnGameSpeedChanged;
        public static Action<GamePlayState> OnUpdateUI;

        // public static Action<bool> OnActiveStartWaveButton;


        public static Action MissionComplete;
        public static Action GameOver;
        public static Action MissionCompleteClick;
    }
}

