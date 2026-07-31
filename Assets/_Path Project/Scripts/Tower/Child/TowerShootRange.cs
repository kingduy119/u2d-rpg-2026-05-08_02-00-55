

using UnityEngine;

namespace TDGame
{
    public class TowerShootRange : MonoBehaviour
    {
        [SerializeField] private Tower _Tower;

        private void OnValidate()
        {
            ShowRadar();
        }

        private void Start()
        {
            ShowRadar();
        }


        private void ShowRadar()
        {
            if (_Tower.TowerSO == null) return;

            float dimeter = _Tower.TowerSO.Ability.ShootRange * 2f;
            transform.localScale = Vector3.one * dimeter;
        }
    }
}