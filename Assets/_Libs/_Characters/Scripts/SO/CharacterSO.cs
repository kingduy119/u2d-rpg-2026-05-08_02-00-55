

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
        public HealthData Health;

        public List<CharacterSetting> colors;
    }

    [Serializable]
    public class HealthData
    {
        public float Health = 100f;
        public float MaxHealth = 100f;
        public float HealthRegen = 1f;
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
        public float DetectionRange = 1f;
    }

    [Serializable]
    public class CharacterSetting //: ScriptableObject
    {
        public Colors color;
        public Sprite Sprite;
        public RuntimeAnimatorController AnimController;
    }

}