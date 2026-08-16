using System;

namespace TDGame
{
    public static class GameMode
    {
#if UNITY_EDITOR
        public static bool IsDev = true;
#else
        public static  bool IsDev = false;
#endif
    }
    public static class GameEvent
    {
        public static Action LoadingDone;

        public static Action<string> LoadScene;

        public static Action PlayContinue;
        public static Action<int> PlayNewGame;

        public static Action PauseGame;
        public static Action ResumeGame;
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