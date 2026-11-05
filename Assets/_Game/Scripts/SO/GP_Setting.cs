

using UnityEngine;

[CreateAssetMenu(fileName = "GP_Setting", menuName = "Game/GP_Setting")]
public class GP_Setting : ScriptableObject
{
    [Header("Population")]
    public int PopulationMin = 3;
    public int PopulationMax = 10;

    [Header("Health")]
    public int Health = 3;
    public int HealthBonus = 0;

    public Resources Resources;
}

