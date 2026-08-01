using System;
using UnityEngine;
using UnityEngine.UI;


namespace TDGame
{

    public class TowerSelectUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_TowerCardPrefab;
        [SerializeField] private GameObject m_TowerSelectList;

        private TowerBoard TowerBoard => GameManager.Instance.TowerBoard;

        private void Awake()
        {
            RefreshUI();
        }



        private void RefreshUI()
        {
            foreach (Transform child in m_TowerSelectList.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var data in TowerBoard.Towers)
            {
                GameObject go = Instantiate(m_TowerCardPrefab, m_TowerSelectList.transform);
                if (go.TryGetComponent<TowerSelectCard>(out var card))
                {
                    card.Initialize(data);

                }
            }

        }

    }
}