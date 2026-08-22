
using UnityEngine;
using System;

namespace TDGame
{
    // public static class InGameEvent
    public static class GamePlayEvent
    {
        public static int Golds = 10;
        public static Action RequestLevelResource;
        public static Action<LevelSO> ResponseLevelResource;

        public static Action RequestUpdateUI;
        public static Action<GamePlayState> ResponseUpdateUI;

        public static Action MainMenuClick;
        public static Action SettingClick;
        public static Action SettingClose;

        // Wave
        public static Action<int> WaveStart;
        public static Action WaveEnd;
        public static Action WaveCompleted;

        public static Action PathwayCount;
        public static Action PathwayEnd;
        public static Action SpawnerCount;
        public static Action SpawnerEnd;

        public static Action<LevelSO> OnLevelLoaded;
        public static Action OnPauseGame;
        public static Action OnGameSpeedChanged;

        // Mission
        public static Action MissionComplete;
        public static Action MissionCompleteClick;
        public static Action GameOver;


        // Tower Cursor Event
        public static Action<Vector3> ShowSelectCursor;
        public static Action HideSelectCursor;
        public static Action<Vector3> ShowBuildCursor;
        public static Action HideBuildCursor;

        public static Action<Tower> BuyTower;

        // Tower UI
        // public static Action 
    }
}

