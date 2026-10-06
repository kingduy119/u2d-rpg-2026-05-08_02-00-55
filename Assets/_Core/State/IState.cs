
using UnityEngine;

public interface IState
{
    void Enter();
    void Execute();
    void Exit();
}

public abstract class State : IState
{
    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void Exit() { }
}


public interface IDamageable
{
    void TakeDamage(float amount);
}

public static class DamageableHelper
{
    public static void CheckCollisionInterfaces(GameObject go, float amount)
    {
        var monoBehaviours = go.GetComponents<MonoBehaviour>();
        foreach (var monoBehaviour in monoBehaviours)
        {
            HandleDamageableInterface(monoBehaviour, amount);
        }
    }

    private static void HandleDamageableInterface(MonoBehaviour monoBehaviour, float amount)
    {
        if (monoBehaviour is IDamageable damageable)
        {
            damageable.TakeDamage(amount);
        }
    }
}