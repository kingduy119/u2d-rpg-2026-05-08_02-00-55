

using UnityEngine;

namespace TDGame
{
    public class TowerBoardEvent : MonoBehaviour
    {
        [SerializeField] private LayerMask _towerLayer;

        private GameObject _tower;

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
                if (_tower.TryGetComponent(out MonoBehaviour mono) && mono is IHoverable hoverable)
                {
                    hoverable.SetHover(true);
                }
            }
            else
            {
                if (_tower != null && _tower.TryGetComponent(out MonoBehaviour mono) && mono is IHoverable hoverable)
                {
                    hoverable.SetHover(false);
                }
            }

        }

        private void HandlePowerUpOnTower()
        {
            if (Input.GetMouseButtonUp(0) && _tower != null)
                GameEvent.RaisePointUpOnTower(_tower);
        }
    }
}