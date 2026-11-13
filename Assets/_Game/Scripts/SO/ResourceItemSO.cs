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

    public event Action<Resources> OnChange;

    public Resources() { }
    public Resources(Resources other)
    {
        if (other == null) return;
        Food = other.Food;
        Gold = other.Gold;
        Wood = other.Wood;
    }

    public void Reset(Resources other = null)
    {
        other ??= new();

        Food = other.Food;
        Gold = other.Gold;
        Wood = other.Wood;
        OnChange?.Invoke(this);
    }

    public void Add(Resources other)
    {
        if (other == null) return;
        Food += other.Food;
        Gold += other.Gold;
        Wood += other.Wood;
        OnChange?.Invoke(this);
    }

    public void Subtract(Resources other)
    {
        if (other == null) return;
        Food -= other.Food;
        Gold -= other.Gold;
        Wood -= other.Wood;
        OnChange?.Invoke(this);
    }

    public bool Enough(Resources required)
    {
        if (required == null) return false;

        return (required.Food <= 0 || Food >= required.Food)
            && (required.Gold <= 0 || Gold >= required.Gold)
            && (required.Wood <= 0 || Wood >= required.Wood);
    }

    public string String()
    {
        return $"Food: {Food} Gold: {Gold} Wood: {Wood}";
    }

    public static bool operator >=(Resources a, Resources b)
    {
        if (a == null || b == null) return false;
        return a.Food >= b.Food
            && a.Gold >= b.Gold
            && a.Wood >= b.Wood;
    }

    public static bool operator <=(Resources a, Resources b)
    {
        if (a == null || b == null) return false;
        return a.Food <= b.Food
            && a.Gold <= b.Gold
            && a.Wood <= b.Wood;
    }

    public static bool operator <(Resources a, Resources b)
    {
        if (a == null || b == null) return false;
        return a.Food < b.Food
            && a.Gold < b.Gold
            && a.Wood < b.Wood;
    }

    public static bool operator >(Resources a, Resources b)
    {
        if (a == null || b == null) return false;
        return a.Food > b.Food
            && a.Gold > b.Gold
            && a.Wood > b.Wood;
    }
}