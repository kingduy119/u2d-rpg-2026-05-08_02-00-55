
using UnityEngine;
using UnityEngine.UI;

namespace Ads
{
    [RequireComponent(typeof(Button))]
    public abstract class AbstractButton : MonoBehaviour
    {
        protected Button _Button;

        protected virtual void Awake()
        {
            _Button = GetComponent<Button>();

        }

        protected virtual void OnEnable()
        {
            _Button.onClick.AddListener(HandleClick);
        }

        protected virtual void OnDisable()
        {
            _Button.onClick.RemoveListener(HandleClick);
        }

        protected abstract void HandleClick();
    }

    public class BannerAdsButton : AbstractButton
    {
        protected override void HandleClick()
        {
            AdsEvent.ShowBanner?.Invoke();
            _Button.interactable = false;
        }
    }

}