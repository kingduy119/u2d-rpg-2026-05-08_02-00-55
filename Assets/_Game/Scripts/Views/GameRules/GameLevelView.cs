using System.Collections.Generic;
using UnityEngine;

public class GameLevelView : MonoBehaviour
{

    int _wave = 0;
    int _level = 0;
    public GamePlayLevelSO Level;
    public bool EndOfWave => _level < Level.Waves.Count;

    public List<PositionCharacter> GetWaveTargets() => Level.Waves[_wave].Targets;

    public bool NextWave()
    {
        _wave++;
        return _wave < Level.Waves.Count;
    }
}


