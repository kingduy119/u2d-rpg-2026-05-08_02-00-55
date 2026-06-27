using System;
using TDGame;
using UnityEngine;

namespace TDGame
{
    public static class GameEvent
    {
        public static event Action<Enemy> OnEnemyDie;

        public static void HandleEnemyDie(Enemy enemy)
        {
            OnEnemyDie(enemy);
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
            }
        }
    }

}