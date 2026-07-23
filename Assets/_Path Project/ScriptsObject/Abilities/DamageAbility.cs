using System;
using UnityEngine;

namespace TDGame
{
    public abstract class Ability : ScriptableObject
    {
        public string _name;
        public Sprite _image;
        public string _description;

        public virtual void Use(GameObject gameObject = null)
        {
            Debug.Log($"Using ability: {_name}");
        }
    }

    [CreateAssetMenu(fileName = "DamageAbility", menuName = "Game TD/Abilities/DamageAbility")]
    public class DamageAbility : Ability
    {
        [SerializeField] private int _physicDamage;
        [SerializeField] private int _magicDamage;

        public override void Use(GameObject gameObject = null)
        {
            base.Use(gameObject);
            Debug.Log($"Damage: {_physicDamage} - {_magicDamage}");
        }
    }
}