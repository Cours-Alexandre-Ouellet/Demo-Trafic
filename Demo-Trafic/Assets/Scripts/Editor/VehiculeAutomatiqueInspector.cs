using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VehiculeAutomatique))]
public class VehiculeAutomatiqueInspector : Editor
{
    private VehiculeAutomatique vehicule;

    public void OnEnable()
    {
        vehicule = (VehiculeAutomatique)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.FloatField("Vitesse", vehicule.Suiveur.Speed);
    }

    public void OnSceneGUI()
    {
        if (vehicule.Suiveur is null || vehicule.Suiveur.Path is null)
        {
            return;
        }

        Path chemin = vehicule.Suiveur.Path;

        // Draws segments and line from controls to anchors
        for (int i = 0; i < chemin.NumSegments; i++)
        {
            Vector3[] points = chemin.GetPointsInSegment(i);
            Handles.color = Color.black;
            Handles.DrawLine(points[1], points[0]);
            Handles.DrawLine(points[2], points[3]);
            Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.green, null, 2.0f);
        }

        // Draws points
        Handles.color = Color.red;
        for (int i = 0; i < chemin.NumPoints; i++)
        {
            Handles.DrawSolidDisc(chemin[i], Vector3.up, 0.2f);
        }
    }
}
