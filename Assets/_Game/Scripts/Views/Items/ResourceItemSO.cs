using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceItemSO", menuName = "Game/ResourceItemSO")]
public class ResourceItemSO : ScriptableObject
{
    public Sprite Icon;
    public int Value;
}

[Serializable]
public class Resources
{
    public int Food;
    public int Gold;
    public int Wood;

    public static bool operator >=(Resources a, Resources b)
    {
        return a.Food >= b.Food
            && a.Gold >= b.Gold
            && a.Wood >= b.Wood;
    }

    public static bool operator <=(Resources a, Resources b)
    {
        return a.Food <= b.Food
            && a.Gold <= b.Gold
            && a.Wood <= b.Wood;
    }
}