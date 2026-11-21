

using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Inventory;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public interface IInventoryService
{
    // UniTask<InventoryData> LoadInventory(string playerId);
}

public class InventoryService : InventoryDB, IAsyncStartable
{
    public async UniTask StartAsync(CancellationToken cancellation = default)
    {
        // var playerId = m_ProfileService.User.UserId;
        // Data = await LoadInventory(playerId).AttachExternalCancellation(cancellation);
    }
}



public class InventoryDB : Service
{
    [Inject] readonly protected DBService m_DBService;
    [Inject] readonly protected ProfileSerice m_ProfileService;

    public List<InventoryData> Data { get; private set; } = new();

    public UniTask<ListInventoryData> LoadInventory(string playerId) =>
    m_DBService.Run(action: async () =>
    {
        var snap = await m_DBService.db.Child("user-inventories").Child(playerId).GetValueAsync().AsUniTask();

        if (!snap.Exists) return null;

        return JsonUtility.FromJson<ListInventoryData>(snap.GetRawJsonValue());

    }, name: "LoadInventory");

    public UniTask<bool> SaveInventory(string playerId, ListInventoryData inventory, Action onSuccess = null) =>
        m_DBService.Run(action: () => m_DBService.db.Child("user-inventories").Child(playerId)
            .SetRawJsonValueAsync(JsonUtility.ToJson(inventory))
            .AsUniTask(),
            name: "SaveInventory",
            onSuccess: onSuccess);
}