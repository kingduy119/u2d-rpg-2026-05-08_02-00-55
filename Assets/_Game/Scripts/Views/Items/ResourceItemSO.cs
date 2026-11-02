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
    public int Food = 0;
    public int Gold = 0;
    public int Wood = 0;

    public Resources() { }
    public Resources(Resources other)
    {
        if (other == null) return;
        Food = other.Food;
        Gold = other.Gold;
        Wood = other.Wood;
    }

    public void Add(Resources other)
    {
        if (other == null) return;
        Food += other.Food;
        Gold += other.Gold;
        Wood += other.Wood;
    }

    public void Minus(Resources other)
    {
        if (other == null) return;
        Food -= other.Food;
        Gold -= other.Gold;
        Wood -= other.Wood;
    }


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

    public static bool operator <(Resources a, Resources b)
    {
        return a.Food < b.Food
            && a.Gold < b.Gold
            && a.Wood < b.Wood;
    }

    public static bool operator >(Resources a, Resources b)
    {
        return a.Food > b.Food
            && a.Gold > b.Gold
            && a.Wood > b.Wood;
    }
}