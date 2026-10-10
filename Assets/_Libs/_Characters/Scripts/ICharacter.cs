

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

    public interface ICterAbstract
    {
        CharacterSO SO { get; set; }

        void Flip();
    }

    public interface ICterAction
    {
        Transform AttackPoint { get; }
        Transform Target { get; }

        void Idle();
        void Move(Vector2 input);
        void Attack(Transform target);
        void Dead();
    }

    public interface ICter : ICterAbstract, ICterAction { }

}