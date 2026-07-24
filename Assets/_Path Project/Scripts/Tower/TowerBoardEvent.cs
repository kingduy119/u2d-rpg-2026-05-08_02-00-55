

using UnityEngine;

namespace TDGame
{
    public class TowerBoardEvent : MonoBehaviour
    {
        [SerializeField] private LayerMask _towerLayer;
        [SerializeField] private TowerSelectCursor _selectOptionsPrefab;

        private GameObject _tower;
        private GameObject _selectedTower;
        private GameObject _prevTower;

        private TowerSelectCursor _towerSelect;
        public TowerSelectCursor TowerSelect
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
            TowerSelect.Deactivate();
        }

        private void Update()
        {
            HandleHoverTower();
            HandlePowerUpOnTower();
        }

        private void HandleHoverTower()
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, Mathf.Infinity, _towerLayer);

            if (hit.collider != null)
            {
                _tower = hit.collider.gameObject;
                if (_tower != _prevTower
                && _tower.TryGetComponent<IHoverable>(out var hoverable))
                {
                    hoverable.SetHover(true);
                    _prevTower = _tower;
                }
            }
            else
            {
                if (_tower != null
                && _tower != _selectedTower
                && _tower.TryGetComponent<IHoverable>(out var hoverable))
                {
                    hoverable.SetHover(false);
                    _tower = null;
                    _prevTower = null;
                }
            }
        }

        private void HandlePowerUpOnTower()
        {
            if (Input.GetMouseButtonUp(0))
            {
                CheckButtonClick();
                CheckTowerClick();
            }

        }

        private Collider2D GetColider()
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, Mathf.Infinity, _towerLayer);
            return hit.collider;
        }

        private void CheckButtonClick()
        {
            Collider2D colider = GetColider();
            if (colider != null && colider.gameObject.TryGetComponent<IClickTrigger>(out var button))
            {
                button.RaiseEvent();
            }
        }

        private void CheckTowerClick()
        {
            if (_tower != null)
            {
                if (_selectedTower != null
                && _selectedTower != _tower
                && _selectedTower.TryGetComponent<IHoverable>(out var hoverable))
                {
                    hoverable.SetHover(false);
                }

                if (_selectedTower != _tower)
                    _selectedTower = _tower;
            }
        }
    }
}