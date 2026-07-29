
using UnityEngine;

namespace TDGame
{
    public interface IState
    {
        public void Enter() { }
        public void Execute() { }
        public void Exit() { }
    }

    public class PointerStateMachine
    {
        public IState CurrentState { get; private set; }
        public PointerHoverState PointerHoverState;
        public PointerBuildTowerState PointerBuildTowerState;

        public PointerStateMachine(WorldMap worldmap)
        {
            PointerHoverState = new(worldmap);
            PointerBuildTowerState = new(worldmap);

            CurrentState = PointerHoverState;
        }

        public void Enable()
        {
            TowerEvent.OnTowerCardSelect += TowerCardSelect;
        }

        public void Disable()
        {
            TowerEvent.OnTowerCardSelect -= TowerCardSelect;
        }

        public void Execute()
        {
            CurrentState?.Execute();
        }

        public void TransitionTo(IState newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            newState.Enter();
            // stateChanged?.Invoke(newState);
        }

        private void TowerCardSelect(TowerSO towerSO)
        {
            PointerBuildTowerState.TowerCardSelect(towerSO);
            TransitionTo(PointerBuildTowerState);
        }
    }

    public class PointerBuildTowerState : IState
    {
        private TowerFactory TowerFactory => GameManager.Instance.FactoryManager.TowerFactory;
        private readonly WorldMap WorldMap;

        private TowerBase _selectedTower;

        public PointerBuildTowerState(WorldMap worldmap)
        {
            WorldMap = worldmap;
        }

        public void Execute()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandlePointerClick();
            }
        }

        private void HandlePointerClick()
        {
            Debug.Log("PointerBuildTowerState.Click");
        }

        public void TowerCardSelect(TowerSO towerSO)
        {
            if (_selectedTower != null)
            {
                _selectedTower.Deactivate();
            }
            _selectedTower = TowerFactory.GetObject(towerSO.towerType);
        }
    }

    public class PointerHoverState : IState
    {
        private GameObject _hoverTower;
        private GameObject _selectedTower;
        private GameObject _prevTower;

        private GameObject TowerSelectCursor => UIManager.Instance.TowerSelectCursor;
        private readonly WorldMap WorldMap;

        public PointerHoverState(WorldMap worldmap)
        {
            WorldMap = worldmap;
        }

        public void Execute()
        {
            HandlePointerHover();

            if (Input.GetMouseButtonUp(0))
            {
                HandlePointerUp();
            }
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
    }

    // End
}