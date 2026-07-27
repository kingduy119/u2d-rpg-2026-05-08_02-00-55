



using UnityEngine;
using UnityEngine.UI;

namespace TDGame
{

    [RequireComponent(typeof(Button))]
    public class ButtonBase : MonoBehaviour
    {
        protected Button _Button;

        protected void Awake()
        {
            _Button = GetComponent<Button>();

        }

        protected void OnEnable()
        {
            _Button.onClick.AddListener(HandleClick);
        }

        protected void OnDisable()
        {
            _Button.onClick.RemoveListener(HandleClick);
        }


        protected virtual void HandleClick()
        {
            TowerEvent.Log("TowerSellButton");
        }
    }
}