using UnityEngine;

[CreateAssetMenu(fileName = "TDEnemyData", menuName = "Game TD/TDEnemyData")]
public class TDEnemyData : ScriptableObject
{
    public float lives;
    public int damage;
    public float moveSpeed;
}
