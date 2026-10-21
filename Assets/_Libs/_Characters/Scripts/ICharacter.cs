

using UnityEngine;
using Weapons;

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
        // ProjectSO ProjectileSO { get; }

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

        void SetTargetLayer(LayerMask layer);
    }

    public interface ICter : ICterAbstract, ICterAction { }

}