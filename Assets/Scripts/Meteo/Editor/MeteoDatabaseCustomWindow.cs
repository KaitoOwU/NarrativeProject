using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        _daySelected = GUILayout.Toolbar(_daySelected,
            new[] { "Dimanche", "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi" });

        GUILayout.Space(20);
        var _tempMeteo = (Meteo)EditorGUILayout.EnumPopup("Meteo", _tempDictionnary[(Day)_daySelected].meteo);
        var _tempMood = (Mood)EditorGUILayout.EnumPopup("Mood", _tempDictionnary[(Day)_daySelected].mood);
        var _tempSubtitle = EditorGUILayout.TextField("Subtitle", _tempDictionnary[(Day)_daySelected].subtitle);
        _tempDictionnary[(Day)_daySelected] = new() { meteo = _tempMeteo, mood = _tempMood, subtitle = _tempSubtitle};

        GUILayout.Space(30);
        GUI.color = Color.green;
        if (GUILayout.Button("Enregistrer la database",
                new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter }))
        {
            _db.Database = (SerializedDictionary<Day, MeteoDatabase.MeteoData>)_tempDictionnary;
            this.Close();
        }

        GUI.color = Color.white;
    }
}
