using UnityEngine;
using Characters;
using Weapons;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private CharacterSO _characterData;
    [SerializeField] private KeyBoardHandler _KeyBoardHandler;

    private void Awake()
    {
        if (_character != null)
        {
            _character.gameObject.tag = "Player";
            _character.gameObject.layer = LayerMask.NameToLayer("Player");
            if (_characterData) _character.SetData(_characterData);
        }
    }

    private void Update()
    {

        if (_KeyBoardHandler.AttackKeyPressed) _character.Attack(null);

        _character.Move(_KeyBoardHandler.AimDirection);
    }

}
