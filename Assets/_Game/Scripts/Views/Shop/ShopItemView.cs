using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemView : ButtonView
{
    [SerializeField] Image _Icon;
    [SerializeField] Image _PurchaseIcon;
    [SerializeField] TMP_Text _RewardValueText;
    [SerializeField] TMP_Text _PriceText;

    [Header("Purchase Type: Ads or IAP")]
    [SerializeField] GameObject _AdsIcon;
    [SerializeField] ItemView _IapItemView;

    [Header("Shop Data")]
    [SerializeField] ShopItemDto _dto;

    protected void OnValidate() => ShowUI();

    protected override void Start()
    {
        ShowUI();
    }

    protected override void HandleClick()
    {
        ShopEvents.ShopItemSelect?.Invoke(_dto);
    }

    protected void ShowUI()
    {
        if (_dto == null) return;

        bool AdsType = _dto is ShopAdsItemDto;

        if (_Icon) _Icon.sprite = _dto.ItemIcon.Icon;
        if (_RewardValueText) _RewardValueText.text = $"{_dto.ItemDesc}";

        if (_PurchaseIcon) _PurchaseIcon.gameObject.SetActive(!AdsType);
        if (_AdsIcon) _AdsIcon.SetActive(AdsType);

        if (_IapItemView)
        {
            _IapItemView.SetActive(!AdsType);
            if (_dto is PurchaseResourceDto purchaseDto)
            {
                _IapItemView.DTO = purchaseDto.PurchaseDTO;
                _IapItemView.SetText(purchaseDto.PurchaseText);
            }
            else if (_dto is ShopIapDto iapDto)
            {
                _IapItemView.DTO = iapDto.PurchaseDTO;
                _IapItemView.SetText(iapDto.PurchaseText);
            }
        }
    }
}
