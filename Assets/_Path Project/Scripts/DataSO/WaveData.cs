using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Game TD/Wave SO")]
public class WaveData : ScriptableObject
{
    public TDEnemyType enemyType;
    public float _spawnTimer;
    public float _spawnInterval = 1f;
    public int perway = 3;
}
