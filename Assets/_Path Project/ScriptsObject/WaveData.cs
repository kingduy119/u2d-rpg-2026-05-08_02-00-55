using UnityEngine;

namespace TDGame
{
    [CreateAssetMenu(fileName = "WaveData", menuName = "Game TD/Wave SO")]
    public class WaveData : ScriptableObject
    {
        public EnemyType enemyType;
        public float _spawnTimer;
        public float _spawnInterval = 1f;
        public int perway = 3;
    }

}