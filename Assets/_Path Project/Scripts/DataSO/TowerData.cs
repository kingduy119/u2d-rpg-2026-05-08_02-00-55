using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Game TD/TowerData")]
public class TowerData : ScriptableObject
{
    public Sprite sprite;
    public GameObject towerPrefab;

    public string towerName;
    public int cost;

    public float range;
    public float shootInterval;
    public float projectileSpeed;
    public float projectileDuration;
    public float damage;

}
