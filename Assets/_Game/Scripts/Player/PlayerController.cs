using UnityEngine;
using Characters;
using Game;
using VContainer;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterSO _characterData;

    private Character _character;
    private GameObject Target;

    [Inject] private IKeyBoardHandler _KeyBoardHandler;
    [Inject] private IControlsSettings _controlsSettings;

    private void Awake()
    {
        if (TryGetComponent<Character>(out var character))
        {
            _character = character;
            _character.gameObject.tag = "Player";
            _character.gameObject.layer = LayerMask.NameToLayer("Player");
            if (_characterData) _character.SO = _characterData;
        }
        _KeyBoardHandler = GameManagerView.Instance.KeyBoard;
    }

    private void Start()
    {
        Debug.Log($"_KeyBoardHandler: {_KeyBoardHandler != null}");
        Debug.Log($"_controlsSettings: {_controlsSettings != null}");
    }

    private void Update()
    {
        if (_KeyBoardHandler == null) return;

        if (_KeyBoardHandler.AttackKeyPressed) _character.Attack(Target?.transform);

        _character.Move(_KeyBoardHandler.AimDirection);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Target = collision.gameObject;
        }
    }

}
