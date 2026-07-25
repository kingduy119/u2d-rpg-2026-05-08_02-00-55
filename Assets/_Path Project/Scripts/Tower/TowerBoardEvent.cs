

using UnityEngine;
using UnityEngine.EventSystems;

namespace TDGame
{
    public class TowerBoardEvent : MonoBehaviour
    {
        [SerializeField] private LayerMask _towerLayer;
        [SerializeField] private GameObject _selectOptionsPrefab;

        private GameObject _tower;
        private GameObject _selectedTower;
        private GameObject _prevTower;

        private GameObject _towerSelect;
        public GameObject TowerSelect
        {
            get
            {
                if (_towerSelect == null)
                    _towerSelect = Instantiate(_selectOptionsPrefab);

                return _towerSelect;
            }
        }

        private void Start()
        {
            TowerSelect.SetActive(false);
        }

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            HandlePointerHover();

            if (Input.GetMouseButtonUp(0))
            {
                HandlePointerUp();
            }
        }

        private void HandlePointerHover()
        {
            Collider2D collider = GetColider(_towerLayer);
            if (collider != null)
            {
                _tower = collider.gameObject;
                if (_tower != _prevTower
                && _tower.TryGetComponent<IHoverable>(out var hoverable))
                {
                    hoverable.SetHover(true);
                    _prevTower = _tower;
                }
            }
            else
            {
                _tower = null;
                if (_prevTower != null
                && _prevTower != _selectedTower
                && _prevTower.TryGetComponent<IHoverable>(out var hoverable))
                {
                    hoverable.SetHover(false);
                    _prevTower = null;
                }
            }
        }

        private void HandlePointerUp()
        {
            Collider2D uiColider = GetColider(LayerMask.GetMask("UI"));

            if (_tower != null)
            {
                CheckTowerClick();
            }

            if (uiColider != null && uiColider.gameObject.TryGetComponent<IClickTrigger>(out var button))
            {
                button.RaiseEvent(_selectedTower);
            }

            if (_tower == null && uiColider == null)
            {
                TowerSelect.SetActive(false);
                if (_selectedTower != null && _selectedTower.TryGetComponent<IHoverable>(out var hoverable))
                {
                    hoverable.SetHover(false);
                    _selectedTower = null;
                }
            }
        }

        private void CheckTowerClick()
        {
            if (_selectedTower != null
            && _selectedTower != _tower
            && _selectedTower.TryGetComponent<IHoverable>(out var hoverable))
            {
                hoverable.SetHover(false);
                TowerSelect.SetActive(false);
            }

            if (_selectedTower != _tower)
            {
                _selectedTower = _tower;
                TowerSelect.transform.position = _selectedTower.transform.position;
                TowerSelect.SetActive(true);
            }
        }

        private Collider2D GetColider(LayerMask layer)
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, Mathf.Infinity, layer);
            return hit.collider;
        }
    }
}