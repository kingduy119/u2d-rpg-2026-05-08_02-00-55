using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace TDGame
{
    public class Platform : MonoBehaviour
    {
        public static event Action<Platform> OnPlatformClicked;
        [SerializeField] private LayerMask layerMask;

        private GameObject _currentTower;

        void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {

                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                RaycastHit2D raycastHit = Physics2D.Raycast(
                    worldPoint,
                    Vector2.zero,
                    Mathf.Infinity,
                    layerMask
                    );

                if (raycastHit.collider != null)
                {
                    Platform platform = raycastHit.collider.GetComponent<Platform>();
                    if (platform != null)
                    {
                        OnPlatformClicked?.Invoke(platform);
                    }
                }
            }
        }

        public void PlaceTower(TowerSO data)
        {
            if (_currentTower != null)
            {
                Destroy(_currentTower);
            }
            _currentTower = Instantiate(data.towerPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}