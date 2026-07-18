using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDGame
{
    public static class GameEvent
    {
        public static event Action OnUpdateUI;

        // InGame
        public static event Action OnLoadLevel;
        public static event Action<TowerSO> OnTowerSelected;

        // Level
        public static event Action<int> OnPlaynewGame;
        public static event Action OnPlayAgain;
        public static event Action OnMissionComplete;

        public static AudioController Audio { get; set; }

        //  Event Functional:
        public static void UpdateUI()
        {
            OnUpdateUI?.Invoke();
        }

        public static void LoadScene(string name)
        {
            if (name == "TD_MainMenu")
            {
                UIManager.Instance.SetupUIMainMenu();
                Audio.PlayMainMenuMusic();
            }
            else
            {
                UIManager.Instance.SetupUIInGame();
                Audio.PlayGameplayMusic();
                OnLoadLevel?.Invoke();
            }
        }

        public static void SendPlayNewgame() => OnPlaynewGame?.Invoke(0);
        public static void SendPlayAgain() => OnPlayAgain?.Invoke();
        public static void SendRestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        public static void SendMissionComplete() => OnMissionComplete?.Invoke();


        public static void HandleTowerSelect(TowerSO data)
        {
            OnTowerSelected?.Invoke(data);
        }

        public static void PauseGame()
        {
            Time.timeScale = 0f;
            Audio.PlayPauseSound();
        }
        public static void ResumeGame()
        {
            Time.timeScale = 1f;
            Audio.PlayResumeSound();
        }

        // Enemy
        public static event Action<Enemy> OnEnemyReachedEnd;
        public static event Action<Enemy> OnGetEnemyReward;
        public static event Action<Enemy> OnEnemyDie;

        public static void SendEnemyReachedEnd(Enemy enemy)
        {
            OnEnemyReachedEnd?.Invoke(enemy);
        }

        public static void SendEnemyDie(Enemy enemy)
        {
            OnEnemyDie?.Invoke(enemy);
            OnGetEnemyReward?.Invoke(enemy);
        }

        public static void PlaySFX(AudioClip clip) => Audio.PlaySoundEffect(clip);
    }

}