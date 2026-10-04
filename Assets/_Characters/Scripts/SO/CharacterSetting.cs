



using UnityEngine;

namespace Characters
{
    // [CreateAssetMenu(fileName = "CharacterSetting", menuName = "Character/CharacterSetting")]
    [System.Serializable]
    public class CharacterSetting //: ScriptableObject
    {
        // [Header("Character Settings")]
        public Colors color;
        public Sprite Sprite;
        public RuntimeAnimatorController AnimController;
    }
}