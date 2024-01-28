using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Kaito.CSVParser;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "MeteoDatabase", menuName = "ScriptableObjects/Meteo Database", order = 1)]
public class MeteoDatabase : ScriptableObject
{

    public SerializedDictionary<Day, MeteoData> Database = new();

    public void Reset()
    {
        EditorUtility.SetDirty(this);
        var dictionary = CSV.Unparse("Assets/Resources/Dialog.csv");
        for (int i = 0; i < 7; i++)
        {
            Database[(Day)i] = new(dictionary.ContainsKey($"DAY{i}_DESC") ? dictionary[$"DAY{i}_DESC"][1] : "not_found");
        }
        AssetDatabase.SaveAssetIfDirty(this);
    }

    public struct MeteoData
    {
        public string subtitle;
        public Meteo meteo;
        public Mood mood;

        public MeteoData(string subtitle = "")
        {
            meteo = Meteo.DEFAULT;
            mood = Mood.HAPPY;
            this.subtitle = subtitle;
        }
        
        public void SetMeteo(Meteo meteo) => this.meteo = meteo;
        public void SetMood(Mood mood) => this.mood = mood;
    }
}

public enum Day
{
    SUNDAY,
    MONDAY,
    TUESDAY,
    WEDNESDAY,
    THURSDAY,
    FRIDAY,
    SATURDAY
}

public enum Meteo
{
    SUNNY,
    DEFAULT,
    RAINY
}

public enum Mood
{
    HAPPY,
    SAD
}