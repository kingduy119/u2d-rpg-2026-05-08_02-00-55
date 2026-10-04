

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(fileName = "CharacterSO", menuName = "Character/CharacterSO")]
    public class CharacterSO : ScriptableObject
    {
        public CterType Type;
        public MoveData Move;
        public CombatData Combat;

        public CterColorData CharacterColors;

        public List<CharacterSetting> colors;
    }

    [Serializable]
    public class MoveData
    {
        public float Speed = 1f;
        public float MaxSpeed = 5f;
    }

    [Serializable]
    public class CombatData
    {
        public float AttackDamage = 1f;
        public float AttackRange = 1f;
        public float AttackSpeed = 1f;
    }

    [Serializable]
    public class CterColorData
    {
        public Sprite BlueSprite;
        public Sprite RedSprite;

        public RuntimeAnimatorController BlueAnimController;
        public RuntimeAnimatorController RedAnimController;
    }

}