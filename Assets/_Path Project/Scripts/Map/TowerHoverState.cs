using UnityEngine;

namespace TDGame
{
    public class TowerHoverState : State
    {
        private readonly WorldMap WorldMap;

        private GameObject _hoverTower;
        private GameObject _selectedTower;
        private GameObject _prevTower;

        public TowerHoverState(WorldMap worldmap)
        {
            WorldMap = worldmap;
        }

        public override void Enter()
        {
            TowerEvent.TowerBuildSlotClick += OnTowerBuildSlotClick;
        }

        public override void Execute()
        {
            HandlePointerHover();

            if (Input.GetMouseButtonUp(0))
            {
                HandlePointerUp();
            }
        }

        public override void Exit()
        {
            TowerEvent.TowerBuildSlotClick -= OnTowerBuildSlotClick;
            GamePlayEvent.HideSelectCursor?.Invoke();

            _prevTower = null;
            _hoverTower = null;
            _selectedTower = null;
        }

        private void OnTowerBuildSlotClick(TowerSO towerSO)
        {
            WorldMap.States.TransitionTo(WorldMap.TowerBuildState);
            TowerEvent.ShowTowerBuild?.Invoke(towerSO);

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
                UnhoverPreviousSelect();
                SelectCurrentHover();
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

        private void SelectCurrentHover()
        {
            if (_selectedTower != _hoverTower)
            {
                _selectedTower = _hoverTower;
                GamePlayEvent.ShowSelectCursor?.Invoke(_selectedTower.transform.position);
            }
        }

        private void UnhoverPreviousSelect()
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