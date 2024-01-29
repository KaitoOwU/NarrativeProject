using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeteoDatabase))]
public class MeteoDatabaseEditor : Editor
{
    private MeteoDatabase Source => (MeteoDatabase)target;

    private void OnEnable()
    {
        if (!Source.IsInit)
        {
            EditorUtility.SetDirty(this);
            Source.Init();
            AssetDatabase.SaveAssetIfDirty(this);
        }
    }

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
