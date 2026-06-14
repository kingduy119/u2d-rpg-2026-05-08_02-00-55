using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Game TD/WayData")]
public class WaveData : ScriptableObject
{
    public PointType pointType;
    public float _spawnTimer;
    public float _spawnInterval = 1f;
    public int perway = 3;
}
