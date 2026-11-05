using UnityEngine;
using VContainer;

public class GameLevelView : MonoBehaviour
{

    int _wave = 0;
    int _level = 0;
    public GamePlayLevelSO LevelSO;
    public bool EndOfWave => _level < LevelSO.Waves.Count;

    public LevelWave Level => LevelSO.Waves[_wave];

    public bool NextWave()
    {
        _wave++;
        return _wave < LevelSO.Waves.Count;
    }
}


