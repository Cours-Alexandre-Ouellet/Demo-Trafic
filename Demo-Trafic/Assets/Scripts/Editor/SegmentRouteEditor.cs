using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SegmentRoute))]
[CanEditMultipleObjects]
public class SegmentRouteEditor : Editor
{
    private SegmentRoute segment;

    public void OnEnable()
    {
        segment = (SegmentRoute)target;
    }

    public void OnSceneGUI()
    {
        Handles.color = Color.white;
        foreach (Vector3 arrives in segment.PointsArrive)
        {
            Handles.DrawSolidDisc(arrives, Vector3.up, 0.2f);
        }

        Handles.color = Color.cyan;
        foreach (Vector3 sorties in segment.PointsSortie)
        {
            Handles.DrawSolidDisc(sorties, Vector3.up, 0.2f);
        }
    }
}
