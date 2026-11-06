

using Characters;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayCardSelect : ButtonView
{
    [SerializeField] CharacterSO _SO;
    [SerializeField] Image _icon;

    [SerializeField] ItemView _food;
    [SerializeField] ItemView _gold;
    [SerializeField] ItemView _wood;

    Resources _resources;

    private void OnValidate()
    {
        UpdateView();
    }

    protected override void HandleClick()
    {
        GamePlayEvents.CterCardSelect?.Invoke(this, _SO, _resources);
    }

    public void Init(CharacterSO SO, Resources resources)
    {
        _SO = SO;
        _resources = resources;
        UpdateView();
        UpdateResourceView();
    }

    private void UpdateView()
    {
        if (_SO && _SO.colors.Count > 0)
            _icon.sprite = _SO.colors[0].Sprite;
    }

    private void UpdateResourceView()
    {
        if (_resources == null) return;
        if (_food != null) _food.VisibleIfZero(_resources.Food);
        if (_gold != null) _gold.VisibleIfZero(_resources.Gold);
        if (_wood != null) _wood.VisibleIfZero(_resources.Wood);
    }
}