using System;
using UnityEngine;
using UnityEngine.UI;


namespace TDGame
{

    public class TowerSelectUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_TowerCardPrefab;
        [SerializeField] private GameObject m_TowerSelectList;
        [SerializeField] private GameObject m_ButtonContain;
        [SerializeField] private Button m_AcceptButton;
        [SerializeField] private Button m_CancelButton;

        public static event Action<Vector3Int, Vector2Int> OnTowerSelecting;
        public static event Action OnAcceptBuild;
        public static event Action OnCancelBuild;

        private void Awake()
        {
            RefreshUI();
        }

        private void OnEnable()
        {
            GameEvent.OnTowerSelected += HandleTowerCardSelect;
            WorldMap.OnAcceptBuildResult += HandleBuildResult;

            m_AcceptButton.onClick.AddListener(HandleAcceptBuildTower);
            m_CancelButton.onClick.AddListener(HandleCancelBuildTower);
        }
        private void OnDisable()
        {
            GameEvent.OnTowerSelected -= HandleTowerCardSelect;
            WorldMap.OnAcceptBuildResult -= HandleBuildResult;

            m_AcceptButton.onClick.RemoveListener(HandleAcceptBuildTower);
            m_CancelButton.onClick.RemoveListener(HandleCancelBuildTower);
        }

        private void RefreshUI()
        {
            foreach (Transform child in m_TowerSelectList.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var data in GameManager.Instance.Towers)
            {
                GameObject go = Instantiate(m_TowerCardPrefab, m_TowerSelectList.transform);
                TowerSelectCard card = go.GetComponent<TowerSelectCard>();
                card.Initialize(data);
            }

            m_ButtonContain.SetActive(false);
        }

        private void HandleTowerCardSelect(TowerSO data) => m_ButtonContain.SetActive(true);

        public void HandleAcceptBuildTower() => OnAcceptBuild?.Invoke();

        public void HandleCancelBuildTower()
        {
            m_ButtonContain.SetActive(false);
            OnCancelBuild?.Invoke();
        }

        private void HandleBuildResult(bool isSuccess)
        {
            if (isSuccess)
            {
                m_ButtonContain.SetActive(false);
            }
            else
            {
                Debug.Log("Play sound cant build");
            }
        }

    }
}