using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static InputController Instance;
    private PlayerInputActions inputActions;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            inputActions = new PlayerInputActions();
        }
        else
            Destroy(gameObject);
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    public Vector2 GetDirection()
    {
        return inputActions.Player.Move.ReadValue<Vector2>();
    }

    public Keyboard GetKeyboard()
    {
        return Keyboard.current;
    }
}
