using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "MeteoDatabase", menuName = "ScriptableObjects/Meteo Database", order = 1)]
public class MeteoDatabase : ScriptableObject
{

    public SerializedDictionary<Day, MeteoData> Database = new();

    public void Reset()
    {
        for (int i = 0; i < 7; i++)
        {
            Database[(Day)i] = new();
        }
    }

    public struct MeteoData
    {
        public string subtitle;
        public Meteo meteo;
        public Mood mood;

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