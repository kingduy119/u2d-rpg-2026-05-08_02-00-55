using System;

namespace TDGame
{
    public static class GameEvent
    {
        public static Action<string> NavigateTo;

        public static Action PlayContinue;
        public static Action<int> PlayNewGame;

        public static Action PauseGame;
        public static Action ResumeGame;

        public static Action SettingOpen;
        public static Action SettingClose;
    }

    public class EnemyEvent
    {
        public static Action EnemySpawn;

        public static Action<Enemy> OnEnemyDie;
        public static Action<Enemy> OnGetEnemyReward;
        public static Action<Enemy> OnEnemyReachedEnd;
    }

    public class PrefabEvent
    {
        public static Action<Tower> LoadTower;
    }

}