



using UnityEngine;
using UnityEngine.UI;

namespace TDGame
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonBase : MonoBehaviour
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
}