using UnityEngine.Pool;

namespace TDGame
{
    public class Tower : TowerBase,
    IPoolable<Tower>
    {
        public IObjectPool<Tower> Pool { get; set; }
        protected TowerStateMachine _TowerState;

        protected override void Awake()
        {
            base.Awake();
            _TowerState = new TowerStateMachine(this);
        }

        public void MarkBuildedState() => _TowerState.TransitionTo(_TowerState.TowerBuildedState);
        public void MarkIdleState() => _TowerState.TransitionTo(_TowerState.TowerIdleState);

        public override void Deactivate() => Pool.Release(this);
    }
}