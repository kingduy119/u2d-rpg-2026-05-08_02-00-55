

using UnityEngine;

namespace Characters
{
    public enum Colors
    {
        Blue,
        Red,
    }

    public enum CterType
    {
        Warrior,
        Archer,
        Lancer
    }

    public interface ICterAction
    {
        void Idle();
        void Move(Vector2 input);
        void Attack(Transform target);
    }

    public interface ICter : ICterAbstract, ICterAction { }
}