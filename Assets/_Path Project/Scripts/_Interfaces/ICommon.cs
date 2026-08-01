using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    public interface IPoolable<T> where T : Component
    {
        IObjectPool<T> Pool { get; set; }
    }

    public interface ITower
    {
        // TowerCombat Combat { get; }
        // TowerHover Hover { get; }

        public void TestFunc() { }
    }


    public interface IEffectTrigger
    {
        void TriggerEffect();
    }

    public interface IDamageable
    {
        void TakeDamage(float amount);
    }

    public interface IHoverable
    {
        void SetHover(bool value);
    }

    public interface IClickTrigger
    {
        public void RaiseEvent(GameObject go = null);
    }

    public interface IState
    {
        public void Enter() { }
        public void Execute() { }
        public void Exit() { }
    }

}