using UnityEngine;

namespace TDGame
{
    public class TowerBoard : MonoBehaviour
    {
        public TowerSO[] Towers;

        [SerializeField] private LayerMask _towerLayer;
        [SerializeField] private GameObject _TowerOptionButtonsPrefab;
    }
}