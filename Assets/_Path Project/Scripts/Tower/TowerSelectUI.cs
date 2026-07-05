using UnityEngine;
using UnityEngine.EventSystems;


namespace TDGame
{
    public class TowerSelectState
    {
        public bool IsTowerSelected { get; private set; } = false;

        public void SelectTower()
        {
            IsTowerSelected = true;
        }

        public void DeselectTower()
        {
            IsTowerSelected = false;
        }
    }
    public class TowerSelectUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_prefab;

        [SerializeField] private GameObject m_testPrefab;
        [SerializeField] private GameObject m_actionButtons;

        private GameObject m_test;

        public Vector3 WorldPosition { get; private set; }
        private TowerSelectState m_state = new();

        private void Awake()
        {
            Refresh();
        }

        private void OnEnable()
        {
            GameEvent.OnTowerSelected += HandleTowerCardSelect;
        }
        private void OnDisable()
        {
            GameEvent.OnTowerSelected -= HandleTowerCardSelect;
        }

        private void HandleTowerCardSelect(TowerSO data)
        {
            if (m_test != null) return;

            m_state.SelectTower();
            m_test = Instantiate(m_testPrefab);
            m_actionButtons.SetActive(true);
        }

        private void Update()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            WorldPosition = mousePos;

            if (EventSystem.current.IsPointerOverGameObject())
                return;
            if (!m_state.IsTowerSelected) return;

            if (Input.GetMouseButtonDown(0))
            {
                m_test.transform.position = WorldPosition;
            }

            if (Input.GetMouseButton(0))
            {
                m_test.transform.position = WorldPosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
            }
        }

        private void Refresh()
        {
            foreach (Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var data in GameManager.Instance.Towers)
            {
                GameObject go = Instantiate(m_prefab, transform);
                TowerSelectCard card = go.GetComponent<TowerSelectCard>();
                card.Initialize(data);
            }

            m_actionButtons.SetActive(false);
        }

        public void HandleCancelBuildTower()
        {
            Destroy(m_test);
            m_actionButtons.SetActive(false);
            m_state.DeselectTower();
        }
        public void HandleAcceptBuildTower()
        {
            TilePlatform platform = m_test.GetComponent<TilePlatform>();
            if (platform.CanBuild)
            {
                m_test = null;
                platform.MarkBuilded();
                m_actionButtons.SetActive(false);
                m_state.DeselectTower();
            }
            else
            {
                Debug.Log("Cant Build");
            }
        }
    }

}