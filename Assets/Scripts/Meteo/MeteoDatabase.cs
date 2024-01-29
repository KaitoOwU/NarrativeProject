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
    public List<DayDatabase> DayDatabase { get; set; } = new();
    
    public bool IsInit { get; private set; }
    public void Init()
    {
        Debug.Log("Init");
        var dictionary = CSV.Unparse("Assets/Resources/Dialog.csv");
        for (int i = 0; i < 7; i++)
        {
            //dayDatabase.Add(new((Day)i, new(dictionary.ContainsKey($"DAY{i}_DESC") ? dictionary[$"DAY{i}_DESC"][1] : "not_found")));

            string desc = string.Empty;
            if (dictionary.Exists((x => x.Item1 == $"DAY{i}_DESC")))
            {
                desc = dictionary.Find(x => x.Item1 == $"DAY{i}_DESC").Item2[0];
            }
            else
            {
                desc = "not_found";
            }
                
            DayDatabase.Add(new((Day)i, new(desc)));
        }

        IsInit = true;
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