using System;
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

        private TowerBase m_selectedTower;
        private TowerSelectState m_state = new();
        public Vector3 WorldPosition { get; private set; }


        private Grid m_grid => GameManager.Instance.WorldMap;


        public static event Action<Vector3Int, Vector2Int> OnTowerSelecting;

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
            if (m_selectedTower != null) return;

            m_selectedTower = FactoryManager.Instance.TowerFactory.GetObject(data.towerType);
            m_actionButtons.SetActive(true);
            m_state.SelectTower();
        }

        private void Update()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            WorldPosition = mousePos;

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (!m_state.IsTowerSelected) return;

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                TestCellPoint();
            }

            if (Input.GetMouseButtonUp(0))
            {
            }
        }

        private void TestCellPoint()
        {

            // Vector3Int origin = m_grid.WorldToCell(WorldPosition);
            // Vector2Int size = m_selectedTower.Size;
            // Vector3 center = m_grid.GetCellCenterWorld(origin);
            // m_selectedTower.transform.position = center;

            Vector3Int origin = m_grid.WorldToCell(WorldPosition);
            Vector2Int size = m_selectedTower.Size;

            Vector3 pos = m_grid.CellToWorld(origin);

            pos += new Vector3(
                size.x * m_grid.cellSize.x * 0.5f,
                size.y * m_grid.cellSize.y * 0.5f,
                0);

            m_selectedTower.transform.position = pos;

            OnTowerSelecting?.Invoke(origin, size);
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
            m_selectedTower.Deactivate();
            m_selectedTower = null;
            m_actionButtons.SetActive(false);
            m_state.DeselectTower();
        }

        public void HandleAcceptBuildTower()
        {
            TowerBase tower = m_selectedTower.GetComponent<TowerBase>();
            if (tower.CanBuild)
            {
                m_selectedTower = null;
                tower.MarkBuilded();
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