using UnityEngine;

public class BottomBarView : ButtonView
{
    [SerializeField] private GameObject _tab;

    protected override void HandleClick()
    {
        Selected();
        ButtonEvents.BottomBarClick?.Invoke(this);
    }

    public override void Selected()
    {
        base.Selected();
        // _Button.interactable = false;
        if (_tab) _tab.SetActive(true);
    }
    public override void UnSelected()
    {
        base.UnSelected();
        // _Button.interactable = true;
        if (_tab) _tab.SetActive(false);
    }
}

