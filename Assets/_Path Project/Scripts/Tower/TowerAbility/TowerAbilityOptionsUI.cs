using System.Collections.Generic;
using UnityEngine;

namespace TDGame
{

    public class TowerAbilityOptionsUI : MonoBehaviour
    {
        [SerializeField] private GameObject _abilityList;
        [SerializeField] private GameObject _abilityCardPrefab;

        public List<Ability> Abilities;

        private void Awake()
        {
            Refresh();
        }

        private void Refresh()
        {
            foreach (Transform child in _abilityList.transform)
            {
                Destroy(child.gameObject);
            }

            if (Abilities.Count <= 0) return;

            foreach (var data in Abilities)
            {
                GameObject go = Instantiate(_abilityCardPrefab, _abilityList.transform);
                TowerAbilityCard card = go.GetComponent<TowerAbilityCard>();
                card.Initialize(data);
            }
        }
    }
}
