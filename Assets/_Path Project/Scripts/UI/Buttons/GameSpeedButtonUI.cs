
using TMPro;
using UnityEngine;

namespace TDGame
{
    public class GameSpeedButtonUI : ButtonBase
    {
        private TMP_Text _GameSpeedText;

        private int _GameSpeed = 1;
        [SerializeField] private int _MaxGameSpeed = 3;

        protected override void Awake()
        {
            base.Awake();
            _GameSpeedText = _Button.GetComponentInChildren<TMP_Text>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void HandleClick()
        {
            _GameSpeed++;
            if (_GameSpeed > _MaxGameSpeed)
            {
                _GameSpeed = 1;
            }
            _GameSpeedText.SetText($"X{_GameSpeed}");
            Time.timeScale = _GameSpeed;
        }

    }
}

