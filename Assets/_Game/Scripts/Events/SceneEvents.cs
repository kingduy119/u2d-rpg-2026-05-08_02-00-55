

using System;
using Characters;

public static class SceneEvents
{
    public static Action<string> ChangeScene;
}

public static class ButtonEvents
{
    public static Action<BottomBarView> BottomBarClick;

    public static Action<CharacterSO> CterDetailSlotView;
}