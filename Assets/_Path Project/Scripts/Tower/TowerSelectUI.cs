using UnityEngine;


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
        private GameObject m_test;

        public Vector3 WorldPosition { get; private set; }
        private TowerSelectState m_state = new TowerSelectState();

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
            Debug.Log($"Tower Selected: {data.name}");
            if (m_test != null) return;

            m_state.SelectTower();
            m_test = Instantiate(m_testPrefab);
        }

        private void Update()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            WorldPosition = mousePos;

            Debug.DrawLine(WorldPosition, WorldPosition + Vector3.up * 0.5f, Color.red);

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
        }
    }

}