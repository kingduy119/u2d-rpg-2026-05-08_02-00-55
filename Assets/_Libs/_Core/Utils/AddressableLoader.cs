

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