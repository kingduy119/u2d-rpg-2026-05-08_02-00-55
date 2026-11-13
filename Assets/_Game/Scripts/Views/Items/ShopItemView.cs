using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemView : ButtonView
{
    [SerializeField] Image _Icon;
    [SerializeField] TMP_Text _RewardValueText;
    [SerializeField] GameObject _AdsIcon;
    [SerializeField] GameObject _IapIcon;

    [SerializeField] ItemSlotSO _ItemSlotSO;

    protected void OnValidate() => ShowUI();

    protected override void HandleClick()
    {
        AdsEvent.ShowRewardAds?.Invoke("ShopItemView.Reward");
        if (_ItemSlotSO != null) ButtonEvents.SaveAdsRewards?.Invoke(_ItemSlotSO.AdsRewards);
    }

    protected void ShowUI()
    {
        if (_AdsIcon) _AdsIcon.SetActive(false);
        if (_IapIcon) _IapIcon.SetActive(false);

        if (_ItemSlotSO == null) return;
        if (_Icon) _Icon.sprite = _ItemSlotSO.ItemIcon;
        if (_RewardValueText) _RewardValueText.text = $"{_ItemSlotSO.Value}";

        if (_ItemSlotSO.Type == ItemSlotType.Ads && _AdsIcon) _AdsIcon.SetActive(true);
        else if (_ItemSlotSO.Type == ItemSlotType.Iap && _IapIcon) _IapIcon.SetActive(true);
    }
}
