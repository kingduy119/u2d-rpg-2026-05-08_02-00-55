

using System;
using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(fileName = "CharacterSO", menuName = "Character/CharacterSO")]
    public class CharacterSO : ScriptableObject
    {
        public MoveData Move;
        public CombatData Combat;

        public CterColorData CharacterColors;
    }

    [System.Serializable]
    public class MoveData
    {
        public float Speed = 1f;
        public float MaxSpeed = 5f;
    }

    [System.Serializable]
    public class CombatData
    {
        public float AttackDamage = 1f;
        public float AttackRange = 1f;
        public float AttackSpeed = 1f;
    }

    [System.Serializable]
    public class CterColorData
    {
        public Sprite BlueSprite;
        public Sprite RedSprite;

        public RuntimeAnimatorController BlueAnimController;
        public RuntimeAnimatorController RedAnimController;
    }

}