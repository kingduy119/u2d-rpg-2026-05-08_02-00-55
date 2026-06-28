using System;

namespace TDGame
{
    public static class GameEvent
    {
        public static event Action OnUpdateUI;
        public static event Action<Enemy> OnEnemyDie;

        public static void UpdateUI()
        {
            OnUpdateUI?.Invoke();
        }

        public static void HandleEnemyDie(Enemy enemy)
        {
            OnEnemyDie?.Invoke(enemy);
        }

        public static void LoadScene(string name)
        {
            if (name == "TD_MainMenu")
            {
                UIController.Instance.SetMainHuD(false);
                AudioManager.Instance.PlayMainMenuMusic();
            }
            else
            {
                UIController.Instance.SetMainHuD(true);
                AudioManager.Instance.PlayGameplayMusic();
                LevelManager.Instance.UpdateLevelResource();
            }
        }
    }

}