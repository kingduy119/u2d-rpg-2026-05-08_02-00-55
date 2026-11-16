using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemView : ButtonView,
IShopItem
{
    [SerializeField] Image _Icon;
    [SerializeField] Image _PurchaseIcon;
    [SerializeField] TMP_Text _RewardValueText;
    [SerializeField] TMP_Text _PriceText;

    [Header("Purchase Type: Ads or IAP")]
    [SerializeField] GameObject _AdsIcon;
    // [SerializeField] GameObject _IapIcon;
    [SerializeField] ItemView _IapItemView;

    [Header("Shop DTO")]
    [SerializeField] ShopItemSO _ShopItemSO;

    protected void OnValidate() => ShowUI();

    protected override void Start()
    {
        ShowUI();
    }

    protected override void HandleClick()
    {
        if (_ShopItemSO != null)
        {
            switch (_ShopItemSO.Type)
            {
                case ItemSlotType.Ads:
                    AdsEvent.ShowRewardAds("RewardAds");
                    ButtonEvents.SaveAdsRewards?.Invoke(_ShopItemSO.Rewards);
                    break;
                case ItemSlotType.Iap:
                    ShopEvents.OnPurchase?.Invoke(_ShopItemSO);
                    break;
                default: break;
            }
        }
    }

    protected void ShowUI()
    {
        if (_ShopItemSO == null) return;

        if (_PurchaseIcon) _PurchaseIcon.gameObject.SetActive(
            _ShopItemSO.Type == ItemSlotType.Iap &&
            _ShopItemSO.PurchasType != ItemType.Currency
        );

        if (_Icon) _Icon.sprite = _ShopItemSO.ItemIcon;

        if (_RewardValueText) _RewardValueText.text = $"{_ShopItemSO.RewardValue}";
        if (_PriceText) _PriceText.text = _ShopItemSO.GetPriceString();

        if (_AdsIcon) _AdsIcon.SetActive(_ShopItemSO.Type == ItemSlotType.Ads);
        if (_IapItemView && _ShopItemSO.Type == ItemSlotType.Iap)
        {
            _IapItemView.DTO = _ShopItemSO.PurchaseDTO;
            _IapItemView.Value = _ShopItemSO.PurchasePrice;
            _IapItemView.SetActive(true);
        }

        // _IapIcon.SetActive(_ShopItemSO.Type == ItemSlotType.Iap);
    }


    public void Init(ShopItemSO so)
    {
        _ShopItemSO = so;
        ShowUI();
    }
}
