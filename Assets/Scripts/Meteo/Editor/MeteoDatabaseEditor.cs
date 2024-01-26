using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeteoDatabase))]
public class MeteoDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUI.enabled = false;
        DrawDefaultInspector();
        GUI.enabled = true;

        if (GUILayout.Button("Edit Database", new GUIStyle(GUI.skin.button){alignment = TextAnchor.MiddleCenter}))
        {
            MeteoDatabaseCustomWindow.Init((MeteoDatabase)target);
        }
    }
}
