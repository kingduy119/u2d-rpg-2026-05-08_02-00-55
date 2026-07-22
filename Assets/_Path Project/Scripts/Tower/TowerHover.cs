

using UnityEngine;

namespace TDGame
{

    public class TowerHover : MonoBehaviour
    {
        private static readonly int IsHoverHash = Animator.StringToHash("IsHover");
        private Animator m_animator;

        public LayerMask towerLayer;
        private GameObject currentHover;

        private Tower _tower;

        private void Awake()
        {
            if (TryGetComponent<Animator>(out var anim))
            {
                m_animator = anim;
            }
            if (TryGetComponent<Tower>(out var tower))
            {
                _tower = tower;
            }
        }

        // private void Update()
        // {
        //     Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, Mathf.Infinity, towerLayer);

        //     if (hit.collider != null)
        //     {
        //         currentHover = hit.collider.gameObject;
        //         if (currentHover.TryGetComponent(out TowerHover newHover))
        //         {
        //             newHover.SetHover(true);
        //         }

        //         if (_tower != null) GameEvent.SendTowerHover(_tower);
        //     }
        //     else
        //     {
        //         if (currentHover != null && currentHover.TryGetComponent(out TowerHover newHover))
        //         {
        //             newHover.SetHover(false);
        //         }

        //         if (_tower != null) GameEvent.SendTowerHover(null);
        //     }

        // }

        public void SetHover(bool value) => m_animator.SetBool(IsHoverHash, value);
    }
}