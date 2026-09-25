using UnityEngine;
using Characters;

public class PlayerController : MonoBehaviour
{
    // [SerializeField] private ControlsSettings _controlsSettings;
    [SerializeField] private Character _character;
    [SerializeField] private KeyBoardHandler _KeyBoardHandler;


    Vector2 direction = Vector2.zero;

    private void Awake()
    {
        if (_character != null)
        {
            _character.gameObject.tag = "Player";
            _character.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

    private void Update()
    {
        direction = _KeyBoardHandler.AimDirection;

        if (_KeyBoardHandler.AttackKeyPressed) _character.Attack();

        _character.Move(direction);
    }

}
