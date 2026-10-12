using System;
using UnityEngine;

public class QuestEvents
{
    public static Action<QuestSO> OnQuestOfferRequested;

    public static Func<QuestSO, bool> IsQuestCompelete;
}
