using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TDGame
{
    public class PointerBuildTowerState : IState
    {
        private FactoryManager FactoryManager => GameManager.Instance.FactoryManager;
        private GameObject TowerPlaceCursor => UIManager.Instance.TowerPlaceCursor;

        private readonly WorldMap WorldMap;
        private TowerBase _SelectedTower;

        public PointerBuildTowerState(WorldMap worldmap)
        {
            WorldMap = worldmap;
        }

        public void Enter()
        {
            TowerEvent.OnAcceptBuild += HandleAcceptBuild;
            TowerEvent.OnCancelBuild += HandleCancelBuild;
        }

        public void Exit()
        {
            TowerEvent.OnAcceptBuild -= HandleAcceptBuild;
            TowerEvent.OnCancelBuild -= HandleCancelBuild;


            if (_SelectedTower != null)
                _SelectedTower.Deactivate();

            _SelectedTower = null;
            TowerPlaceCursor.SetActive(false);
            WorldMap.HiddenTilemapPreview();
        }

        public void Execute()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandlePointerClick();
            }
        }

        public void HandleTowerCardSelect(TowerSO towerSO)
        {
            if (_SelectedTower != null)
                _SelectedTower.Deactivate();

            _SelectedTower = FactoryManager.GetTower(towerSO.towerType);

            Vector3 centerWorld = Camera.main.ViewportToWorldPoint(
                new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));

            _SelectedTower.transform.position = centerWorld;
            TowerEvent.OnTowerPlace?.Invoke(_SelectedTower);

            EnablePlaceCursor();
        }

        private void HandlePointerClick()
        {
            if (_SelectedTower == null) return;

            Collider2D collider = WorldMap.GetColider(LayerMask.GetMask("UI"));
            if (collider != null)
            {
                IClickTrigger[] triggers = collider.gameObject.GetComponents<IClickTrigger>();
                foreach (var trigger in triggers)
                    trigger.RaiseEvent();
            }
            else
            {
                _SelectedTower.transform.position = WorldMap.WorldPosition;
                TowerEvent.OnTowerPlace?.Invoke(_SelectedTower);
                EnablePlaceCursor();
            }

        }

        private void EnablePlaceCursor()
        {
            TowerPlaceCursor.transform.position = _SelectedTower.transform.position;
            TowerPlaceCursor.SetActive(true);
        }

        private void HandleAcceptBuild()
        {
            if (WorldMap.CanBuild)
            {
                _SelectedTower = null;
                WorldMap.AcceptBuild();
                ChangeHoverState();
            }
            else
            {
                Debug.Log("Can not BUILD");
            }
        }
        private void HandleCancelBuild() => ChangeHoverState();
        private void ChangeHoverState() => WorldMap.PointerStateMachine.TransitionTo(WorldMap.PointerStateMachine.PointerHoverState);
    }

}