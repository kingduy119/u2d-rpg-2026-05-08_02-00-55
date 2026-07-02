using System;
using UnityEngine;

namespace TDGame
{
    public static class GameEvent
    {
        public static event Action OnUpdateUI;
        public static event Action<Enemy> OnEnemyDie;

        public static event Action OnPauseInGame;
        public static event Action OnResumeInGame;

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
                UIManager.Instance.SetupUIMainMenu();
                AudioManager.Instance.PlayMainMenuMusic();
            }
            else
            {
                UIManager.Instance.SetupUIInGame();
                AudioManager.Instance.PlayGameplayMusic();
                LevelManager.Instance.UpdateLevelResource();
            }
        }

        public static void TowerSelect(TowerSO data)
        {
            Debug.Log("TowerSelect");
        }

        public static void PauseGame()
        {
            Time.timeScale = 0f;
            AudioManager.Instance.PlayPauseSound();
        }
        public static void ResumeGame()
        {
            Time.timeScale = GameManager.Instance.InGame.GameSpeed;
            AudioManager.Instance.PlayResumeSound();
        }
    }

}