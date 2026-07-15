using UnityEngine;

namespace TDGame
{
    public class SpawnState : DirtyState
    {
        private int _spawnCount = 0;
        private float _timer = 0f;
        private float _interval = 1f;

        public void UpdateSpawn(WaveData data)
        {
            _interval = data._spawnInterval;
            _timer = data._spawnInterval;
            _spawnCount = 0;
        }

        public bool ForEachTimer(int perway)
        {
            _timer -= Time.deltaTime;
            return _timer <= 0f && _spawnCount < perway;
        }

        public void RefreshTimer()
        {
            _timer = _interval;
            _spawnCount++;
        }

        public void ResetCount()
        {
            _spawnCount = 0;
        }
    }
}