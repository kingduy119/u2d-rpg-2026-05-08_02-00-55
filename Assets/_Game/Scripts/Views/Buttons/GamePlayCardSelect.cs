

using Characters;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayCardSelect : ButtonView
{
    [SerializeField] CharacterSO _SO;
    [SerializeField] Image _icon;

    private void OnValidate()
    {
        UpdateView();
    }

    protected override void HandleClick()
    {
        ButtonEvents.GamePlayCardSelect?.Invoke(this, _SO);
    }

    private void UpdateView()
    {
        if (_SO && _SO.colors.Count > 0)
        {
            _icon.sprite = _SO.colors[0].Sprite;
        }
    }
}