using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    public interface IPoolable<T> where T : Component
    {
        IObjectPool<T> Pool { get; set; }
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

}