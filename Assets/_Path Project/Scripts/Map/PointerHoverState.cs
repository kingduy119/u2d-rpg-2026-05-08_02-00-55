using UnityEngine;

namespace TDGame
{
    public class PointerHoverState : IState
    {
        private readonly WorldMap WorldMap;

        private GameObject _hoverTower;
        private GameObject _selectedTower;
        private GameObject _prevTower;

        public PointerHoverState(WorldMap worldmap)
        {
            WorldMap = worldmap;
        }

        public void Enter()
        {
            _hoverTower = null;
            _selectedTower = null;
            _prevTower = null;
        }

        public void Execute()
        {
            HandlePointerHover();

            if (Input.GetMouseButtonUp(0))
            {
                HandlePointerUp();
            }
        }

        public void Exit()
        {
            GamePlayEvent.HideSelectCursor?.Invoke();
            UnhoverSelectedTower();
        }

        private void HandlePointerHover()
        {
            Collider2D collider = WorldMap.GetColider(LayerMask.GetMask("Object"));
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
            {
                UnhoverSelectedTower();
                SelectTower();
            }
            else
            {
                GamePlayEvent.HideSelectCursor?.Invoke();
                if (_selectedTower != null && _selectedTower.TryGetComponent<IHoverable>(out var hoverable))
                {
                    hoverable.SetHover(false);
                    _selectedTower = null;
                }
            }
        }

        private void SelectTower()
        {
            if (_selectedTower != _hoverTower)
            {
                _selectedTower = _hoverTower;
                GamePlayEvent.ShowSelectCursor?.Invoke(_selectedTower.transform.position);
            }
        }

        private void UnhoverSelectedTower()
        {
            if (_selectedTower != null
            && _selectedTower != _hoverTower
            && _selectedTower.TryGetComponent<IHoverable>(out var hoverable))
            {
                hoverable.SetHover(false);
            }
        }
    }

}