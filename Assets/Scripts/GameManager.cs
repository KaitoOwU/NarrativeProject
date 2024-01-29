using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Kaito.CSVParser;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<(string, string[])> _dialogDatabase = new();
    
    public static GameManager Instance { get; private set; }
    
    public Language GameLanguage { get; private set; } = Language.ENGLISH;
    public List<(string, string[])> DialogDatabase
    {
        get => _dialogDatabase;
    }

    public Day Day { get; private set; } = Day.SUNDAY;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
        Instance = this;
        _dialogDatabase = CSV.Unparse("Assets/Resources/Dialog.csv");
    }

    public string GetDialog(string id) => DialogDatabase.Find(x => x.Item1 == id).Item2[(int)GameLanguage];
    public MeteoData GetCurrentDayData() => Resources.Load<MeteoDatabase>("Meteo/MeteoDatabase").DayDatabase[(int)Day].meteoData;
}
