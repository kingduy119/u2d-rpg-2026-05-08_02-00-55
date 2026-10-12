using System.Collections.Generic;
using UnityEngine;

public class LocationHistoryTracker : MonoBehaviour
{
    private readonly HashSet<LocationSO> locationsVisited = new();

    public void RecordLocation(LocationSO locationSO)
    {
        locationsVisited.Add(locationSO);
    }

    public bool HasVisited(LocationSO locationSO)
    {
        return locationsVisited.Contains(locationSO);
    }
}
