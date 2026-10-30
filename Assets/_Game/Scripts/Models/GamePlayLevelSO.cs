

using System;
using System.Collections.Generic;
using Characters;
using UnityEngine;



[Serializable]
public class PositionCharacter
{
    public int position;
    public CharacterSO character;
}

[Serializable]
public class LevelWave
{
    public List<PositionCharacter> Targets = new();
}

[CreateAssetMenu(fileName = "GamePlayLevelSO", menuName = "Game/GamePlayLevelSO")]
public class GamePlayLevelSO : ScriptableObject
{
    public List<LevelWave> Waves = new();
}