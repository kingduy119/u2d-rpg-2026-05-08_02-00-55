using UnityEngine;

namespace TDGame
{
    public class FactoryManager : PersistentSingleton<FactoryManager>
    {
        [SerializeField] public EnemyFactory EnemyFactory;
        [SerializeField] public TowerFactory TowerFactory;

        protected override void Awake()
        {
            base.Awake();
            // EnemyFactory = GetComponent<EnemyFactory>();
            // TowerFactory = GetComponent<TowerFactory>();
        }
    }
}