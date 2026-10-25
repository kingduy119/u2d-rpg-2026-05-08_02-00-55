using UnityEngine;
using Characters;
using System.Collections.Generic;
using VContainer;
using TMPro;

public class CharacterDetailTabView : MonoBehaviour
{
    [Header("Views")]
    [SerializeField] TMP_Text _damageView;
    [SerializeField] TMP_Text _amorView;
    [SerializeField] TMP_Text _healthView;
    // [SerializeField] TMP_Text _goldView;

    private void UpdateDetailView()
    {
        if (_currentSO == null) return;
        _damageView.text = $"{_currentSO.Combat.AttackDamage} Physics Damage";
        _amorView.text = $"{_currentSO.Combat.PhysicsAmor} Physics Amor";
        _healthView.text = $"{_currentSO.Health.MaxHealth} Health";
    }

    [SerializeField] CterItemSlotView _slotPrefab;
    [SerializeField] Transform _listParent;

    public List<CharacterSO> _characters;
    public CharacterSO _currentSO;

    private List<ButtonView> _slots = new();
    private ButtonView _prevButton;


    [Header("Render Character View Port")]
    [SerializeField] private Transform _cameraViewPoint;
    private Character _cterSelected;

    [Inject] FactoryService _factory;

    private void Start()
    {
        RefreshCharacterList();

        if (_characters.Count > 0 && _slots.Count > 0)
        {
            _currentSO = _characters[0];
            _prevButton = _slots[0];
            _prevButton.Selected();

            UpdateView();
        }
    }

    private void OnEnable()
    {
        ButtonEvents.SelectByCharacterSO += ButtonEvents_SelectByCharacterSO;
    }

    private void OnDisable()
    {
        ButtonEvents.SelectByCharacterSO -= ButtonEvents_SelectByCharacterSO;
    }


    private void ButtonEvents_SelectByCharacterSO(ButtonView button, CharacterSO cterSO)
    {
        _currentSO = cterSO;

        if (_prevButton) _prevButton.UnSelected();
        _prevButton = button;

        UpdateView();
    }

    private void UpdateView()
    {
        if (_factory == null || _currentSO == null) return;

        var character = _factory.GetPrefab(_currentSO);
        if (character)
        {
            if (_cterSelected) _cterSelected.Deactive();

            character.gameObject.transform.position = _cameraViewPoint.position;
            character.gameObject.layer = LayerMask.NameToLayer("Character");
            _cterSelected = character;
        }

        UpdateDetailView();
    }

    private void RefreshCharacterList()
    {
        foreach (Transform child in _listParent)
            Destroy(child.gameObject);

        if (_characters.Count > 0)
        {
            foreach (var cterSO in _characters)
            {
                var slot = Instantiate(_slotPrefab, _listParent);
                slot.Init(cterSO);
                _slots.Add(slot);
            }
        }
    }
}
