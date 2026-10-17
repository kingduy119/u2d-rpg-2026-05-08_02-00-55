using UnityEngine;
using Characters;
using System.Collections.Generic;

public class CharacterDetailTabView : MonoBehaviour
{


    [SerializeField] CterItemSlotView _slotPrefab;
    [SerializeField] Transform _listParent;

    public List<CharacterSO> _characters;
    public CharacterSO _currentSO;

    private List<CterItemSlotView> _slots = new();

    private void Start()
    {
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
