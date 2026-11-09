


using UnityEngine;



public class AuthButtonView : ButtonView
{
    [SerializeField] private AuthMethods authEvent;
    protected override void HandleClick()
    {
        ButtonEvents.OnAuth?.Invoke(authEvent);
    }
}