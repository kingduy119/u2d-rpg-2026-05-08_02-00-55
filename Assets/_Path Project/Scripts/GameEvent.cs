using System;
using UnityEngine;

namespace TDGame
{
    public static class GameEvent
    {
        public static event Action OnUpdateUI;
        public static event Action<Enemy> OnEnemyDie;

        // InGame
        public static event Action OnLoadLevel;
        public static event Action OnMissionComplete;

        public static event Action<TowerSO> OnTowerSelected;

        public static void UpdateUI()
        {
            OnUpdateUI?.Invoke();
        }

        public static void LoadScene(string name)
        {
            if (name == "TD_MainMenu")
            {
                UIManager.Instance.SetupUIMainMenu();
                AudioManager.Instance.PlayMainMenuMusic();
            }
            else
            {
                UIManager.Instance.SetupUIInGame();
                AudioManager.Instance.PlayGameplayMusic();
                OnLoadLevel?.Invoke();
            }
        }

        public static void SendMissionComplete() => OnMissionComplete?.Invoke();
        public static void HandleEnemyDie(Enemy enemy)
        {
            OnEnemyDie?.Invoke(enemy);
        }

        public static void HandleTowerSelect(TowerSO data)
        {
            OnTowerSelected?.Invoke(data);
        }

        public static void PauseGame()
        {
            Time.timeScale = 0f;
            AudioManager.Instance.PlayPauseSound();
        }
        public static void ResumeGame()
        {
            Time.timeScale = 1f;
            AudioManager.Instance.PlayResumeSound();
        }

        // Enemy
        public static event Action<EnemyData> OnEnemyReachedEnd;
        public static event Action<EnemyData> OnGetEnemyReward;
        public static void SendEnemyReachedEnd(EnemyData enemySO)
        {
            OnEnemyReachedEnd?.Invoke(enemySO);
        }

        public static void SendEnemyReward(EnemyData enemySO)
        {
            OnGetEnemyReward?.Invoke(enemySO);
        }
    }

}