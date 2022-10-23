using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ZoneArretFeu))]
public class ZoneArretFeuInspector : Editor
{
    ZoneArretFeu zoneArret;

    public void OnEnable()
    {
        zoneArret = (ZoneArretFeu)target;
    }

    public void OnSceneGUI()
    {
        Debug.Log("Hello");
        Handles.DrawLine(zoneArret.transform.position, zoneArret.transform.position + zoneArret.direction, 10f);
    }
}
