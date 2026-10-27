
using UnityEngine;

public class GamePlayButtonView : ButtonView
{
    [SerializeField] string _eventName;
    protected override void HandleClick()
    {
        ButtonEvents.Gameplay?.Invoke(_eventName);
    }
}