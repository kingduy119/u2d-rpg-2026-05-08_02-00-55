using UnityEngine;
using Characters;
using Game;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterSO _characterData;

    private Character _character;
    private KeyBoardHandler _KeyBoardHandler;

    private void Awake()
    {
        if (TryGetComponent<Character>(out var character))
        {
            _character = character;
            _character.gameObject.tag = "Player";
            _character.gameObject.layer = LayerMask.NameToLayer("Player");
            if (_characterData) _character.SetData(_characterData);
        }
        _KeyBoardHandler = GameManagerView.Instance.KeyBoard;
    }

    private void Update()
    {

        if (_KeyBoardHandler.AttackKeyPressed) _character.Attack(null);

        _character.Move(_KeyBoardHandler.AimDirection);
    }

}
