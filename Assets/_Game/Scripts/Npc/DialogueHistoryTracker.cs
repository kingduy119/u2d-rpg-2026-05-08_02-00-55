using System.Collections.Generic;
using UnityEngine;

public class DialogueHistoryTracker : MonoBehaviour
{
    public static DialogueHistoryTracker Instance;
    private readonly HashSet<ActorSO> spokerNPCs = new();


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RecordNPC(ActorSO actorSO)
    {
        spokerNPCs.Add(actorSO);
    }

    public bool HasSpokenWith(ActorSO actorSO)
    {
        return spokerNPCs.Contains(actorSO);
    }
}
