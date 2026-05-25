using System.Collections.Generic;
using UnityEngine;

public class LocationHistoryTracker : MonoBehaviour
{
    public static LocationHistoryTracker Instance;
    private readonly HashSet<LocationSO> locationsVisited = new();


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RecordLocation(LocationSO locationSO)
    {
        locationsVisited.Add(locationSO);
    }

    public bool HasVisited(LocationSO locationSO)
    {
        return locationsVisited.Contains(locationSO);
    }
}
