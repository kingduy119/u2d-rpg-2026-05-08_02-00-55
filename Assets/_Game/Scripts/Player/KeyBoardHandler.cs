using UnityEngine;


public interface IKeyBoardHandler
{
    bool AttackKeyPressed { get; }
    Vector2 AimDirection { get; }
}

public class KeyBoardHandler : IKeyBoardHandler
{
    private readonly IControlsSettings _controlsSettings;

    public KeyBoardHandler(IControlsSettings controller)
    {
        Debug.Log("KeyBoardHandler.KeyBoardHandler");
        _controlsSettings = controller;
    }

    public bool AttackKeyPressed => Input.GetKeyDown(_controlsSettings.AttackKey);
    public Vector2 AimDirection => GetAimDirection();


    private Vector2 GetAimDirection()
    {
        Vector2 direction = Vector2.zero;

        if (Input.GetKey(_controlsSettings.AimLeftKey))
            direction.x = -1f;
        else if (Input.GetKey(_controlsSettings.AimRightKey))
            direction.x = 1f;
        if (Input.GetKey(_controlsSettings.AimUpKey))
            direction.y = 1f;
        else if (Input.GetKey(_controlsSettings.AimDownKey))
            direction.y = -1f;

        return direction;
    }
}