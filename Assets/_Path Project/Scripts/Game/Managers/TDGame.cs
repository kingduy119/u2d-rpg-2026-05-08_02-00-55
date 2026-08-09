using UnityEngine;

namespace TDGame
{
    public class TDGame<T> : PersistentSingleton<T>
        where T : MonoBehaviour
    {
        [SerializeField] protected LevelManager _levelManagerPrefab;
        [SerializeField] protected TowerBoard _towerBoardPrefab;
        [SerializeField] protected AudioController AudioController;

        protected LevelManager _levelManager;
        protected FactoryManager _factoryManager;
        protected TowerBoard _towerBoard;
    }
}
