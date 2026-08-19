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
        public static Action LoadingSceneDone;

        public static Action<string> LoadScene;

        public static Action PlayContinue;
        public static Action<int> PlayNewGame;

        public static Action PauseGame;
        public static Action ResumeGame;
    }

    public class EnemyEvent
    {
        public static Action EnemySpawn;

        public static Action<Enemy> EnemyDie;
        public static Action<Enemy> ReceiveReward;
        public static Action<Enemy> ReachedEnd;
    }

    public class PrefabEvent
    {
        public static Action<Tower> LoadTower;
    }

}