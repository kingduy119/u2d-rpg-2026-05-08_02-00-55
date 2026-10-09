

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Weapons;

public class AddressableLoader
{
    private List<string> _labels;
    private AsyncOperationHandle<IList<GameObject>> handle;
    private Addressables.MergeMode _mode = Addressables.MergeMode.Union;

    public AddressableLoader() { }
    public AddressableLoader(IEnumerable<string> labels)
    {
        _labels = labels.ToList();
    }
    public AsyncOperationHandle<IList<GameObject>> LoadAssetsAsync()
    {
        if (handle.IsValid()) return handle;
        handle = Addressables.LoadAssetsAsync<GameObject>(_labels, null, _mode);
        return handle;
    }

    public void Release()
    {
        if (handle.IsValid())
            Addressables.Release(handle);
        handle = default;
    }
}