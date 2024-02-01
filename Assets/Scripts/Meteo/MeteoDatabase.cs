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
    [SerializeField] public List<DayDatabase> dayDatabase = new();

    private void Awake()
    {
        var dictionary = CSV.Unparse("Assets/Resources/Dialog.csv");
        for (int i = 0; i < 7; i++)
        {
            string desc = string.Empty;
            desc = dictionary.Exists((x => x.Item1 == $"DAY{i}_DESC")) ? dictionary.Find(x => x.Item1 == $"DAY{i}_DESC").Item2[0] : "not_found";
            dayDatabase.Add(new((Day)i, new(desc)));
        }
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
    NEUTRAL,
    SAD,
    REALLY_SAD,
    LOW_ANGRY,
    ANGRY,
    BLUSHING,
    IN_LOVE,
    AWKWARD
}

[Serializable]
public struct DayDatabase
{
    public Day day;
    public MeteoData meteoData;

    public DayDatabase(Day day, MeteoData meteoData)
    {
        this.day = day;
        this.meteoData = meteoData;
    }
}

[Serializable]
public struct MeteoData
{
    public Meteo meteo;
    public Mood mood;

    public MeteoData(string subtitle = "")
    {
        meteo = Meteo.DEFAULT;
        mood = Mood.HAPPY;
    }
        
    public void SetMeteo(Meteo meteo) => this.meteo = meteo;
    public void SetMood(Mood mood) => this.mood = mood;
}