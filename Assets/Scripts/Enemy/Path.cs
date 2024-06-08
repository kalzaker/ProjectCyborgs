using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Path
{
    public List<Transform> waypoints = new List<Transform>();

    [SerializeField] bool alwaysDrawPath;
    [SerializeField] bool drawAsLoop;
    [SerializeField] bool drawNumbers;

    [SerializeField] Color debugColor = Color.white;
#if UNITY_EDITOR

    public void OnDrawGizmos()
    {
        if (alwaysDrawPath)
            DrawPath();
    }
    public void DrawPath()
    {
        for(int i = 0; i < waypoints.Count; i++)
        {
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.fontSize = 30;
            labelStyle.normal.textColor = debugColor;
            if(drawNumbers)
                Handles.Label(waypoints[i].position, i.ToString(), labelStyle);
            if (i >= 1)
            {
                Gizmos.color = debugColor;
                Gizmos.DrawLine(waypoints[i-1].position, waypoints[i].position);

                if (drawAsLoop)
                    Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position);
            }
        }
    }
    public void OnDrawGizmosSelected()
    {
        if (alwaysDrawPath)
            return;
        else
            DrawPath();
    }
#endif
}
