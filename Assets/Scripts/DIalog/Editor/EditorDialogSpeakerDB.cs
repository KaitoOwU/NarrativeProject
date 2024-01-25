using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogSpeakerDatabase))]
public class EditorDialogSpeakerDB : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Update DB Automatically", new GUIStyle(GUI.skin.button){alignment = TextAnchor.MiddleCenter}))
        {
            DialogSpeakerDatabase db = (DialogSpeakerDatabase)target;
            
            var list = new List<DialogSpeakerData>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(DialogSpeakerData)));

            for( int i = 0; i < guids.Length; i++ )
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                DialogSpeakerData asset = AssetDatabase.LoadAssetAtPath<DialogSpeakerData>(assetPath);
                if(asset != null)
                {
                    list.Add(asset);
                }
            }
            db.Speakers = list;
        }
    }
}
