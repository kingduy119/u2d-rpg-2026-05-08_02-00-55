using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemView : ButtonView
{
    [SerializeField] Image _icon;
    [SerializeField] TMP_Text _text;

    protected override void HandleClick()
    {
        AdsEvent.ShowRewardAds?.Invoke("ShopItemView.Reward");
    }
}
