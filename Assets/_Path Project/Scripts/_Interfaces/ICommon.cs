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

    public interface IState
    {
        void Enter() { }
        void Execute() { }
        void Exit() { }
    }

    public interface IState2
    {
        IEnumerator Execute();
        void Enter();
        void Exit();
        void AddLink(ILink link);
        void RemoveLink(ILink link);
        bool ValidateLinks(out IState2 nextState);
        void EnableLinks();
        void DisableLinks();
    }

    public interface ILink
    {
        bool Validate(out IState2 nextState);
        void Enable() { }
        void Disable() { }
    }



}