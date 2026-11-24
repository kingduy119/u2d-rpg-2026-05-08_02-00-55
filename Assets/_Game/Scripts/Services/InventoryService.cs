

using System;
using System.Collections.Generic;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using InventoryLib;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class InventoryService : InventoryDB
{
    [Inject] readonly InventoryView _InventoryView;

    public void AddItem(UserData user, RewardItem reward)
    {
        if (_InventoryView == null) return;

        var data = new InventoryData
        {
            Id = reward.Item.Id,
            Stack = reward.Value
        };
        Data.Add(data);
        SaveInventory(
            user.UserId, new ListInventoryData { Datas = Data.ToArray() },
            onSuccess: () => _InventoryView.SetData(Data.ToArray())
        ).Forget();
    }

    public async UniTask Load(string userId)
    {
        var data = await LoadInventory(userId);

        if (data != null) Data = new(data.Datas);
        else Data = new();

        _InventoryView.SetData(Data.ToArray());
    }
}

public class InventoryDB : Service
{
    [Inject] readonly protected DBService m_DBService;
    [Inject] readonly protected ProfileSerice m_ProfileService;

    public List<InventoryData> Data { get; protected set; }

    public UniTask<ListInventoryData> LoadInventory(string playerId) =>
    m_DBService.Run(action: async () =>
    {
        var snap = await m_DBService.db.Child("user-inventories").Child(playerId).GetValueAsync().AsUniTask();

        if (!snap.Exists) return null;

        return JsonUtility.FromJson<ListInventoryData>(snap.GetRawJsonValue());

    }, name: "LoadInventory");

    public UniTask<bool> SaveInventory(
        string playerId,
        ListInventoryData inventory,
        Action onSuccess = null) =>
        m_DBService
            .Run(action: () => m_DBService.db.Child("user-inventories").Child(playerId)
            .SetRawJsonValueAsync(JsonUtility.ToJson(inventory))
            .AsUniTask(),
            name: "SaveInventory",
            onSuccess: onSuccess);
}