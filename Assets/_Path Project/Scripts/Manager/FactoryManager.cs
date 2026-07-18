using UnityEngine;

namespace TDGame
{
    public class FactoryManager : PersistentSingleton<FactoryManager>
    {
        public EnemyFactory EnemyFactory;
        public TowerFactory TowerFactory;
        public ProjectileFactory ProjectileFactory;



        protected override void Awake()
        {
            base.Awake();
        }
    }
}