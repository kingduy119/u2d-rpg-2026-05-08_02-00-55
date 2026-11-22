


using System.Collections.Generic;
using IAPLib;
using UnityEngine;

public class MainMenuShopView : MonoBehaviour
{
    [SerializeField] ShopItemView _shopItemPrefab;
    [SerializeField] Transform _shopItemContainer;

    public List<ShopItemDto> Dtos;

    void Start()
    {
        // ShopEvents.ShopItemSelect += OnShopItemSelect;
        ClearShopItems();
        FillShopItems();
    }

    void FillShopItems()
    {
        foreach (var dto in Dtos)
        {
            if (dto == null) continue;

            var itemView = Instantiate(_shopItemPrefab, _shopItemContainer);
            itemView.Init(dto);
        }
    }

    void ClearShopItems()
    {
        foreach (Transform child in _shopItemContainer)
        {
            Destroy(child.gameObject);
        }
    }

    void OnShopItemSelect(ShopItemDto dto)
    {
        Debug.Log($"Selected Shop Item: {dto.ItemName}");
    }
}