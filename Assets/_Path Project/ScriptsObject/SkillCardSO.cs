using System;
using UnityEngine;

namespace TDGame
{
    [CreateAssetMenu(fileName = "SkillCardSO", menuName = "Game TD/SkillCardSO")]
    public class SkillCardSO : ScriptableObject
    {
        public Sprite Thumbnail;
        public string Title;
        public string Description;
        public CombatAbility Ability;
    }

    [Serializable]
    public class CombatAbility
    {
        public int AttackSpeed;
        public int AttackRange;
        public int PhysicDamage;
        public int MagicDamage;
    }
}