using UnityEngine;
using System.Collections;

namespace TDGame
{
    public class TowerBuildState : State
    {
        private GameManager GameManager => GameManager.Instance;

        private readonly WorldMap WorldMap;
        private Tower _SelectedTower;

        public TowerBuildState(WorldMap worldmap)
        {
            WorldMap = worldmap;
        }

        public override void Enter()
        {
            TowerEvent.TowerBuildSlotClick += OnTowerBuildSlotClick;
            TowerEvent.ShowTowerBuild += OnShowTowerBuild;

            TowerEvent.AcceptBuild += OnAcceptBuild;
            TowerEvent.OnCancelBuild += HandleCancelBuild;
        }

        public override void Exit()
        {
            if (_SelectedTower != null)
            {
                _SelectedTower.Deactivate();
                _SelectedTower = null;
            }

            GamePlayEvent.HideBuildCursor?.Invoke();
            WorldMap.HiddenTilemapPreview();

            TowerEvent.TowerBuildSlotClick -= OnTowerBuildSlotClick;
            TowerEvent.ShowTowerBuild -= OnShowTowerBuild;

            TowerEvent.AcceptBuild -= OnAcceptBuild;
            TowerEvent.OnCancelBuild -= HandleCancelBuild;
        }

        public override void Execute()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandlePointerClick();
            }
        }

        private void OnTowerBuildSlotClick(TowerSO towerSO) => Coroutines.StartCoroutine(LoadTower(towerSO));

        private void OnShowTowerBuild(TowerSO towerSO)
        {
            Coroutines.StartCoroutine(LoadTower(towerSO));
        }

        private IEnumerator LoadTower(TowerSO towerSO)
        {
            if (_SelectedTower != null && _SelectedTower.SO != towerSO)
            {
                _SelectedTower.Deactivate();
                _SelectedTower = null;
            }

            if (_SelectedTower == null)
            {
                _SelectedTower = GameManager.FactoryManager.GetTower(towerSO);
                _SelectedTower.MarkIdleState();

                yield return null;
            }

            Vector3 centerWorld = Camera.main.ViewportToWorldPoint(
                new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));

            _SelectedTower.transform.position = centerWorld;

            TowerEvent.TowerPlace?.Invoke(_SelectedTower);
            GamePlayEvent.ShowBuildCursor?.Invoke(_SelectedTower.transform.position);

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
                _SelectedTower.transform.position = WorldMap.MouseWorldPosition;
                TowerEvent.TowerPlace?.Invoke(_SelectedTower);

                GamePlayEvent.ShowBuildCursor?.Invoke(_SelectedTower.transform.position);
            }

        }

        private void OnAcceptBuild()
        {
            if (WorldMap.CanBuild)
            {
                _SelectedTower.MarkBuildedState();
                _SelectedTower = null;
                WorldMap.AcceptBuild();
                ChangeHoverState();
                GamePlayEvent.BuyTower?.Invoke(_SelectedTower);
            }
            else
            {
                Debug.Log("Can not BUILD");
                // 1. Play sound
            }
        }
        private void HandleCancelBuild() => ChangeHoverState();
        private void ChangeHoverState() => WorldMap.States.TransitionTo(WorldMap.TowerHoverState);
    }

}