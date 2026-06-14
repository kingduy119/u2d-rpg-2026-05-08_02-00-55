using UnityEditor;
using UnityEngine;

public class Path : MonoBehaviour
{
    public GameObject[] wayPoints;

    public Vector3 GetPointPosition(int index)
    {
        return wayPoints[index].transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < wayPoints.Length - 1; i++)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.alignment = TextAnchor.MiddleCenter;
            Handles.Label(wayPoints[i].transform.position, wayPoints[i].name, style);

            Gizmos.DrawLine(wayPoints[i].transform.position, wayPoints[i + 1].transform.position);
        }
    }
}
