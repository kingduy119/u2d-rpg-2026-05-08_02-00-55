using System;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class Point : MonoBehaviour
{
    [SerializeField] private PointData pointData;
    public static event Action<PointData> OnPointReachedEnd;

    private Path currentPath;
    private Vector3 _targetPosition;
    private int _pathIndex = 0;

    void Awake()
    {
        currentPath = GameObject.Find("Path1").GetComponent<Path>();
    }

    void OnEnable()
    {
        _pathIndex = 0;
        _targetPosition = currentPath.GetPointPosition(_pathIndex);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            _targetPosition,
            pointData.moveSpeed * Time.deltaTime);

        float distance = (transform.position - _targetPosition).magnitude;
        if (distance < 0.1f)
        {
            if (_pathIndex >= currentPath.wayPoints.Length - 1)
            {
                // Reached the end of the path
                OnPointReachedEnd?.Invoke(pointData);
                gameObject.SetActive(false);
                return;
            }
            _pathIndex++;
            _targetPosition = currentPath.GetPointPosition(_pathIndex);
        }
    }
}
