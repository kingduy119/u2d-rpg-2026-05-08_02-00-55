using System;
using Core;

public class UserData
{
    public string UserId;
    public string Name;
    public string Nickname;
    public int Level;
    public bool FirstLogin = true;
}

[Serializable]
public class ResourceData
{
    public int Diamond = 0;
    public int Gold = 0;

    public void Add(RewardItem reward)
    {
        switch (reward.Item.Type)
        {
            case ItemType.Diamond:
                Diamond += reward.Value;
                break;
            case ItemType.Gold:
                Gold += reward.Value;
                break;
        }
    }

    public static ResourceData operator +(ResourceData a, ResourceData b)
    {
        return new ResourceData
        {
            Diamond = a.Diamond + b.Diamond,
            Gold = a.Gold + b.Gold
        };
    }
    public static ResourceData operator -(ResourceData a, ResourceData b)
    {
        return new ResourceData
        {
            Diamond = a.Diamond - b.Diamond,
            Gold = a.Gold - b.Gold
        };
    }
    public static bool operator >=(ResourceData a, ResourceData b)
    {
        return a.Diamond >= b.Diamond && a.Gold >= b.Gold;
    }
    public static bool operator <=(ResourceData a, ResourceData b)
    {
        return a.Diamond <= b.Diamond && a.Gold <= b.Gold;
    }
    public static bool operator >(ResourceData a, ResourceData b)
    {
        return a.Diamond > b.Diamond && a.Gold > b.Gold;
    }
    public static bool operator <(ResourceData a, ResourceData b)
    {
        return a.Diamond < b.Diamond && a.Gold < b.Gold;
    }
}