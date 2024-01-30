using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Kaito.CSVParser;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Dictionary<string, string[]> _dialogDatabase = new();
    
    public static GameManager Instance { get; private set; }
    
    public Language GameLanguage { get; private set; } = Language.ENGLISH;
    public ReadOnlyDictionary<string, string[]> DialogDatabase { get => new(_dialogDatabase); }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _dialogDatabase = CSV.Unparse("Assets/Resources/Dialog.csv");
    }

    public string GetDialog(string id) => DialogDatabase[id][(int)GameLanguage];
}
