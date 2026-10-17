using System.Collections.Generic;
using UnityEngine;

public class MainMenuView : MonoBehaviour
{
    public List<BottomBarView> buttons;
    BottomBarView active;

    void Start()
    {
        if (buttons.Count > 0)
        {
            active = buttons[0];
            active.Selected();
        }
    }

    void OnEnable()
    {
        ButtonEvents.BottomBarClick += ButtonEvents_BottomBarClick;
    }

    void OnDisable()
    {
        ButtonEvents.BottomBarClick -= ButtonEvents_BottomBarClick;
    }


    private void ButtonEvents_BottomBarClick(BottomBarView button)
    {
        if (active) active.UnSelected();
        active = button;
    }
}
