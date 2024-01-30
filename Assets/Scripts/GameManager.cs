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

    public Language GameLanguage { get; private set; } = Language.FRANCAIS;
    public ReadOnlyDictionary<string, string[]> DialogDatabase { get => new(_dialogDatabase); }

    public List<int> _emotions;
    private int[] _lastTwoEmotions = new int[2];


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _emotions = new List<int> { 0, 0, 0, 0};
        _dialogDatabase = CSV.Unparse("Assets/Resources/Dialog.csv");
    }

    public void ResetEmotions()
    {
        _emotions.Clear();
    }

    public string GetDialog(string id) => DialogDatabase[id][(int)GameLanguage];
}
