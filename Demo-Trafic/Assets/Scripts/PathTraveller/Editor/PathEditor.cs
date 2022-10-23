/**
 * Aadpté de 
 * LAGUE S, [Unity] 2D Curve Editor (E01 : introduction and concepts), télévsersée le 21 janvier 2018 (https://www.youtube.com/watch?v=RF04Fi9OCPc)
 * LAGUE S, [Unity] 2D Curve Editor (E02 : adding and moving points), télévsersée le 25 janvier 2018 (https://www.youtube.com/watch?v=n_RHttAaRCk) 
 * LAGUE S, [Unity] 2D Curve Editor (E03 : closed path and auto-controls), télévsersée le 21 janvier 2018 (https://www.youtube.com/watch?v=nNmFLWup4_k) 
 * 
 * Alexandre Ouellet - 9 octobre 2022
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

/// <summary>
/// Creating paths in editor
/// </summary>
[CustomEditor(typeof(PathManager))]
public class PathEditor : Editor
{
    // References to objects
    private PathManager manager;
    private Path path;

    // Internal variables
    private int currentSelectedPath;
    private bool flagWarningName = false;

    /// <summary>
    /// Draws controls in inspector and reacts to user inputs
    /// </summary>
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        // Path manager
        int selectedPathIndex = EditorGUILayout.Popup("Path selected", currentSelectedPath, manager.pathCollection.Select(p => p.Name).ToArray());
        if (currentSelectedPath != selectedPathIndex)
        {
            Undo.RecordObject(manager, "Change selected path");
            currentSelectedPath = selectedPathIndex;
            manager.path = manager.pathCollection[currentSelectedPath];
            path = manager.path;
            flagWarningName = false;
        }

        string pathName = EditorGUILayout.TextField("Name", path.Name);
        if (pathName != path.Name)
        {
            Undo.RecordObject(manager, "Change path name");
            if(!manager.Rename(currentSelectedPath, pathName))
            {
                flagWarningName = true;
            }
            else
            {
                flagWarningName = false;
            }
        }

        if(flagWarningName)
        {
            EditorGUILayout.HelpBox("Name already in use", MessageType.Warning, true);
        }

        if (GUILayout.Button("Delete path"))
        {
            Undo.RecordObject(manager, "Delete path");
            manager.pathCollection.Remove(path);
            if (manager.pathCollection.Count == 0)
            {
                manager.CreatePath();
            }

            currentSelectedPath = 0;
            manager.path = manager.pathCollection[currentSelectedPath];
            path = manager.path;
            flagWarningName = false;
        }

        if (GUILayout.Button("Create new"))
        {
            Undo.RecordObject(manager, "Create new path");
            manager.CreatePath();
            path = manager.path;
            currentSelectedPath = manager.pathCollection.Count - 1;
            flagWarningName = false;
        }

        EditorGUILayout.Space(30f);

        // Path parameters
        if (GUILayout.Button("Toggle closed"))
        {
            Undo.RecordObject(manager, "Toggle closed");
            path.ToggleClosed();

        }

        bool autoSetControlPoints = GUILayout.Toggle(path.AutoSetControlPoints, "Auto set control points");
        if (autoSetControlPoints != path.AutoSetControlPoints)
        {
            Undo.RecordObject(manager, "Toggle auto set controls");
            path.AutoSetControlPoints = autoSetControlPoints;
        }

        float level = EditorGUILayout.FloatField("Level", manager.level);
        if(manager.level != level)
        {
            Undo.RecordObject(manager, "Change level");
            manager.level = level;
        }

        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }

    /// <inheritdoc/>
    private void OnSceneGUI()
    {
        Input();
        Draw();
    }

    /// <summary>
    /// Manages input from event in world space
    /// </summary>
    private void Input()
    {
        Event guiEvent = Event.current;

        // Get ray from screen point to intercept plane XZ leveled at float value specified in manager.
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
        float t = (manager.level - mouseRay.origin.y) / mouseRay.direction.y;
        Vector3 intersectionPoint = mouseRay.origin + t * mouseRay.direction;
        Vector3 spawnPosition = mouseRay.GetPoint(Vector3.Distance(mouseRay.origin, intersectionPoint));

        // Shift left-click to add a point
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            Undo.RecordObject(manager, "Add segment");
            path.AddSegment(spawnPosition);
        }
    }

    /// <summary>
    /// Draws the bezier's curves, and anchors and controls points
    /// </summary>
    private void Draw()
    {
        // Draws segments and line from controls to anchors
        for (int i = 0; i < path.NumSegments; i++)
        {
            Vector3[] points = path.GetPointsInSegment(i);
            Handles.color = Color.black;
            Handles.DrawLine(points[1], points[0]);
            Handles.DrawLine(points[2], points[3]);
            Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.green, null, 2.0f);
        }

        // Draws points
        Handles.color = Color.red;
        for (int i = 0; i < path.NumPoints; i++)
        {
            Vector3 newPosition = Handles.FreeMoveHandle(path[i], Quaternion.identity, .1f, Vector3.zero, Handles.CylinderHandleCap);
            if (path[i] != newPosition)
            {
                Undo.RecordObject(manager, "Move point");
                path.MovePoint(i, newPosition);
            }
        }
    }

    /// <inheritdoc/>
    private void OnEnable()
    {
        manager = (PathManager)target;
        if(manager.pathCollection is null)
        {
            manager.pathCollection = new List<Path>();
        }
        
        if (manager.path is null)
        {
            manager.CreatePath();
        }
        path = manager.path;
        SceneView.RepaintAll();
    }
}
