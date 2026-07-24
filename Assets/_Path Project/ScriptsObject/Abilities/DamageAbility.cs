using UnityEngine;

namespace TDGame
{
    [CreateAssetMenu(fileName = "DamageAbility", menuName = "Game TD/Abilities/DamageAbility")]
    public class DamageAbility : Ability
    {
        [SerializeField] private int _physicDamage;
        [SerializeField] private int _magicDamage;

        public override void Use(GameObject gameObject = null)
        {
            Debug.Log($"Damage: {_physicDamage} - {_magicDamage}");
        }
    }
}