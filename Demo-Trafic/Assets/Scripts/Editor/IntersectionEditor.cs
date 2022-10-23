using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Intersection))]
[CanEditMultipleObjects]
public class IntersectionEditor : Editor
{
    private Intersection intersection;

    public void OnEnable()
    {
        intersection = (Intersection)target;
    }

    public void OnSceneGUI()
    {
        Handles.color = Color.white;
        foreach (Vector3 arrives in intersection.PointsArrive)
        {
            Handles.DrawSolidDisc(arrives, Vector3.up, 0.2f);
        }

        Handles.color = Color.cyan;
        foreach (Vector3 sorties in intersection.PointsSortie)
        {
            Handles.DrawSolidDisc(sorties, Vector3.up, 0.2f);
        }
    }
}
