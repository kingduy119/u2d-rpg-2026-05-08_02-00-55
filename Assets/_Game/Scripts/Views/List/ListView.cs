

using System;
using System.Collections.Generic;
using UnityEngine;

public class ListView : MonoBehaviour
{
    public List<GameObject> GetItems()
    {
        List<GameObject> result = new();
        foreach (Transform child in transform)
            result.Add(child.gameObject);

        return result;
    }

    public List<T> GetItemInChild<T>(string path) where T : Component
    {
        List<T> result = new();
        foreach (Transform child in transform)
        {
            Transform item = child.Find(path);

            if (item != null && item.TryGetComponent<T>(out var component))
            { result.Add(component); }
        }

        return result;
    }

    public List<T> LoopItemInChild<T>(string name = "Item", Action<T> callback = null) where T : Component
    {
        List<T> result = new();

        foreach (Transform child in transform)
        {
            var obj = child.Find(name);
            if (obj == null || !obj.TryGetComponent<T>(out var item)) continue;

            callback?.Invoke(item);
            result.Add(item);
        }
        return result;
    }

}