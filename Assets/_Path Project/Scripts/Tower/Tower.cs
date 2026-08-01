using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    public class Tower : TowerBase,
    IPoolable<Tower>,
    ITower
    {
        public IObjectPool<Tower> Pool { get; set; }

        protected TowerStateMachine _StateMachine;


        protected override void Awake()
        {
            base.Awake();
            _StateMachine = new TowerStateMachine(this);
        }
        // public override void TowerUP()
        // {
        //     if (TowerSO.NextTowerLevel != null)
        //     {
        //         TowerBase tower = TestFactory.GetObject(TowerSO.NextTowerLevel);
        //         tower.gameObject.transform.position = transform.position;

        //         // TowerBase tower = TestFactory.GetObject(TowerSO.NextTowerLevel);
        //         // tower.gameObject.transform.position = transform.position;

        //         Deactivate();
        //     }
        // }

        public override void Deactivate() => Pool.Release(this);
    }
}