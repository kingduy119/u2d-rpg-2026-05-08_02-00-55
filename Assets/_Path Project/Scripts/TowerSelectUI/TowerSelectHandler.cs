
using UnityEngine;
using UnityEngine.EventSystems;

namespace TDGame
{
    public class TowerSelectHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask _towerLayer;
        [SerializeField] private GameObject _TowerSelectCursorPrefab;

        private GameObject _hoverTower;
        private GameObject _selectedTower;
        private GameObject _prevTower;

        private GameObject _TowerSelectCursor;
        public GameObject TowerSelectCursor
        {
            get
            {
                if (_TowerSelectCursor == null)
                    _TowerSelectCursor = Instantiate(_TowerSelectCursorPrefab, gameObject.transform);

                return _TowerSelectCursor;
            }
        }

        private void OnEnable()
        {
            TowerEvent.OnAbilitySelect += HandleAbilitySelect;
            TowerEvent.OnSelectUpdateTower += UpdateSelectedTower;
        }

        private void OnDisable()
        {
            TowerEvent.OnAbilitySelect -= HandleAbilitySelect;
            TowerEvent.OnSelectUpdateTower -= UpdateSelectedTower;
        }

        private void UpdateSelectedTower()
        {
            if (_selectedTower != null
            && _selectedTower.TryGetComponent<Tower>(out var tower))
            {
                tower.TowerUP();
            }
        }

        private void HandleAbilitySelect(Ability ability)
        {
            if (_selectedTower != null)
            {
                ability.Apply(_selectedTower);
            }
        }

        private void Update()
        {
            // if (EventSystem.current.IsPointerOverGameObject())
            //     return;

            // HandlePointerHover();

            // if (Input.GetMouseButtonUp(0))
            // {
            //     HandlePointerUp();
            // }
        }

        private void HandlePointerHover()
        {
            Collider2D collider = GetColider(_towerLayer);
            if (collider != null)
            {
                _hoverTower = collider.gameObject;
                if (_hoverTower != _prevTower
                && _hoverTower.TryGetComponent<IHoverable>(out var hoverable))
                {
                    hoverable.SetHover(true);
                    _prevTower = _hoverTower;
                }
            }
            else
            {
                _hoverTower = null;
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
            if (_hoverTower != null)
                CheckTowerClick();
            else
            {
                TowerSelectCursor.SetActive(false);
                if (_selectedTower != null && _selectedTower.TryGetComponent<IHoverable>(out var hoverable))
                {
                    hoverable.SetHover(false);
                    _selectedTower = null;
                }
            }
        }

        private void CheckTowerClick()
        {
            // Unhover prev tower
            if (_selectedTower != null
            && _selectedTower != _hoverTower
            && _selectedTower.TryGetComponent<IHoverable>(out var hoverable))
            {
                hoverable.SetHover(false);
            }

            if (_selectedTower != _hoverTower)
            {
                _selectedTower = _hoverTower;
                TowerSelectCursor.transform.position = Camera.main.WorldToScreenPoint(_selectedTower.transform.position);
                TowerSelectCursor.SetActive(true);
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