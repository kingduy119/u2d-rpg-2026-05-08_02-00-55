

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Weapons;

public class AddressableLoader
{

    private AsyncOperationHandle<IList<GameObject>> handle;
    private Addressables.MergeMode _mode = Addressables.MergeMode.Union;

    public AddressableLoader() { }
    // public AddressableLoader(string[] labels)
    // {
    //     _labels = labels.ToArray();
    // }
    public AsyncOperationHandle<IList<GameObject>> LoadAssetsAsync(string[] labels)
    {
        if (handle.IsValid()) return handle;
        handle = Addressables.LoadAssetsAsync<GameObject>(labels, null, _mode);
        return handle;
    }

    public void Release()
    {
        if (handle.IsValid())
            Addressables.Release(handle);
        handle = default;
    }
}

public static class SizeFormatter
{
    static readonly string[] Units = { "B", "KB", "MB", "GB", "TB" };

    public static string Format(long bytes)
    {
        double size = bytes;
        int unit = 0;

        while (size >= 1024 && unit < Units.Length - 1)
        {
            size /= 1024;
            unit++;
        }

        // Byte thì không cần số thập phân
        return unit == 0 ? $"{bytes} {Units[0]}" : $"{size:F2} {Units[unit]}";
    }
}