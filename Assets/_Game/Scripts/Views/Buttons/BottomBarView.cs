using UnityEngine;


// Main menu bottom
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
        if (_tab) _tab.SetActive(true);
    }
    public override void UnSelected()
    {
        base.UnSelected();
        if (_tab) _tab.SetActive(false);
    }
}

