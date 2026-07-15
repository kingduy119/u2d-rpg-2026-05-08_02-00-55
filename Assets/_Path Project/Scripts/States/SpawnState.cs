using UnityEngine;

namespace TDGame
{
    public class SpawnState : DirtyState
    {
        // public bool IsDirty { get; private set; } = false;
        private int _count = 0;
        private int _waveCount = 0;
        private float _timer = 0f;
        private float _interval = 1f;
        private int _removedEnemies = 0;
        public int WaveCount
        {
            get => _waveCount;
            set
            {
                _waveCount = value;
                IsDirty = true;
            }
        }
        public int EnemyCount
        {
            get => _removedEnemies;
            set
            {
                _removedEnemies = value;
                IsDirty = true;
            }
        }

        public bool ForEachTimer(int perway)
        {
            _timer -= Time.deltaTime;
            return _timer <= 0f && _count < perway;
        }

        public void RefreshTimer()
        {
            _timer = _interval;
            _count++;
        }

        public void ResetNewWave()
        {
            WaveCount++;
            _removedEnemies = 0;
            _count = 0;
        }
        // public void HandlePointReachedEnd(EnemyData pointData)
        // {
        //     EnemyCount++;
        // }

        // public void HandleEnemyDie(Enemy enemy)
        // {
        //     EnemyCount++;
        // }
    }
}