


using UnityEngine;

public enum LoginMethods
{
    Anonymously,
    Unity
}

public class AuthButtonView : ButtonView
{
    [SerializeField] private LoginMethods LoginMethod;
    protected override void HandleClick()
    {
        ButtonEvents.Login?.Invoke(LoginMethod);
    }
}