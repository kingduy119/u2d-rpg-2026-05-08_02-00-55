


using Characters;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterCardSO", menuName = "Game/CharacterCardSO")]
public class CharacterCardSO : ScriptableObject
{
    public CharacterSO CharacterSO;
    public Resources Resources;
}