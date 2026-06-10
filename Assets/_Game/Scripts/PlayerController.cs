using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private BaseMovement m_BaseMovement;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }
    void OnEnable()
    {
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Start()
    {
        m_BaseMovement = GetComponent<BaseMovement>();
    }

    void Update()
    {
        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        if (!Mathf.Approximately(moveInput.x, 0.0f) || !Mathf.Approximately(moveInput.y, 0.0f))
        {
            m_BaseMovement.SetDirection(moveInput);
            m_BaseMovement.SetState(State.Moving);
        }
        else
        {
            m_BaseMovement.SetState(State.Idle);
        }

    }
}
