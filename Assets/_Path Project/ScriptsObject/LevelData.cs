using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game TD/LevelData")]
public class LevelData : ScriptableObject
{
    public string levelName;
    // public Sprite levelThumbnail;
    public string sceneName;
    public int startingLives;
    public int startingGold;

    // public AudioClip backgroundMusic;
    public WaveData[] waves;
}
