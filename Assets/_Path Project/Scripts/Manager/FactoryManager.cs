using UnityEngine;

namespace TDGame
{
    public class FactoryManager : PersistentSingleton<FactoryManager>
    {
        [SerializeField] public EnemyFactory EnemyFactory;
        [SerializeField] public TowerFactory TowerFactory;
        [SerializeField] public ProjectileFactory ProjectileFactory;

        protected override void Awake()
        {
            base.Awake();
        }
    }
}