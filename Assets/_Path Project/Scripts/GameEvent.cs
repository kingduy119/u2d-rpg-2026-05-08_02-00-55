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
    }

}