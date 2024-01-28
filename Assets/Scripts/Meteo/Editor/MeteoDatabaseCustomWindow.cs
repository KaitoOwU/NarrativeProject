using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kaito.CSVParser;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class MeteoDatabaseCustomWindow : EditorWindow
{
    private static MeteoDatabase _db;

    private int _daySelected = 0;
    private static Dictionary<Day, MeteoDatabase.MeteoData> _tempDictionnary = new();

    public static void Init(MeteoDatabase value)
    {
        _db = value;
        _tempDictionnary = _db.Database;
        MeteoDatabaseCustomWindow window = GetWindowWithRect<MeteoDatabaseCustomWindow>(new Rect(0, 0, 500, 700));
        window.Show();
    }

    private void OnGUI()
    {
        EditorUtility.SetDirty(Resources.Load<MeteoDatabase>("Meteo/MeteoDatabase"));
        _daySelected = GUILayout.Toolbar(_daySelected,
            new[] { "Dimanche", "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi" });

        GUILayout.Space(20);
        var _tempMeteo = (Meteo)EditorGUILayout.EnumPopup("Meteo", _tempDictionnary[(Day)_daySelected].meteo);
        var _tempMood = (Mood)EditorGUILayout.EnumPopup("Mood", _tempDictionnary[(Day)_daySelected].mood);

        GUI.enabled = false;
        EditorGUILayout.TextField("Subtitle", _tempDictionnary[(Day)_daySelected].subtitle);
        GUI.enabled = true;
        if (GUILayout.Button("Open CSV file", new GUIStyle(GUI.skin.button){alignment = TextAnchor.MiddleCenter}))
        {
            Application.OpenURL(Application.streamingAssetsPath + "../Resources/Dialog.csv");
        }
        
        _tempDictionnary[(Day)_daySelected] = new(_tempDictionnary[(Day)_daySelected].subtitle){meteo = _tempMeteo, mood = _tempMood};
        GUILayout.Space(30);
        GUI.color = Color.green;
        if (GUILayout.Button("Enregistrer la database",
                new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter }))
        {
            _db.Database = (SerializedDictionary<Day, MeteoDatabase.MeteoData>)_tempDictionnary;
            AssetDatabase.SaveAssetIfDirty(Resources.Load<MeteoDatabase>("Meteo/MeteoDatabase"));
            this.Close();
        }

        GUI.color = Color.white;
    }
}
