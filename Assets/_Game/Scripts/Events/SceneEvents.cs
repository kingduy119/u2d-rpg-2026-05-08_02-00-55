

using System;
using Characters;
using UnityEngine;

public static class SceneEvents
{
    public static Action<string> ChangeScene;
}

public static class ButtonEvents
{
    public static Action<BottomBarView> BottomBarClick;

    public static Action<ButtonView, CharacterSO> SelectByCharacterSO;

    public static Action<string> GamePlayEvents;
    public static Action<ButtonView, CharacterSO> GamePlayCardSelect;
}

public static class FactoryEvents
{
    public static Action<CharacterSO, Transform> SpawnCharacter;
}

public static class GameDefenseEvents
{
    public static Action<ITargetController> ChangeTilemapPosition;
}


public static class GamePlayEvents
{
    public static Action<string, TargetController> AddTarget;
}
