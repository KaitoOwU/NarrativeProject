using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DG.Tweening;
using Kaito.CSVParser;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static readonly Dictionary<Day, string> DayName = new()
    {
        { Day.SATURDAY, "Saturday" },
        { Day.MONDAY, "Monday" },
        { Day.TUESDAY, "Tuesday" },
        { Day.WEDNESDAY, "Wednesday" },
        { Day.THURSDAY, "Thursday" },
        { Day.FRIDAY, "Friday" },
        { Day.SUNDAY, "Sunday" }
    };
    
    [SerializeField] private Image _fade;
    private List<(string, string[])> _dialogDatabase = new();
    
    public static GameManager Instance { get; private set; }
    
    public Language GameLanguage { get; private set; } = Language.ENGLISH;
    public List<(string, string[])> DialogDatabase
    {
        get => _dialogDatabase;
    }

    public Day Day { get; private set; } = Day.SUNDAY;

    public Action<Language> OnGameLanguageChange;

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

    public void StartUwu() => StartCoroutine(CR_Start());

    public IEnumerator CR_Start()
    {
        var task = SceneManager.LoadSceneAsync(DayName[Day] + "Scene");
        yield return new WaitUntil(() => task.isDone);
        
        UIManager.Instance.Fade.DOFade(0f, 1f);
    }

    public string GetDialog(string id)
    {
        var find = DialogDatabase.Find(x => x.Item1 == id);
        if (find == default)
            return "not_found";
        
        return find.Item2[(int)GameLanguage];
    }

    public MeteoData GetCurrentDayData()
    {
        var yes = Resources.Load<MeteoDatabase>("Meteo/MeteoDatabase").dayDatabase;
        return yes[(int)Day].meteoData;
    }

    public void SetLanguage(Language value)
    {
        GameLanguage = value;
        OnGameLanguageChange.Invoke(GameLanguage);
    }

    public IEnumerator CR_EndScenario(Action callback)
    {
        yield return UIManager.Instance.Fade.DOFade(1f, 1f).WaitForCompletion();
        callback.Invoke();
        yield return new WaitForSecondsRealtime(3f);
        yield return UIManager.Instance.Fade.DOFade(0f, 1f).WaitForCompletion();
    }
    
    public IEnumerator CR_EndDay()
    {
        yield return UIManager.Instance.Fade.DOFade(1f, 1f).WaitForCompletion();
        Day++;
        yield return SceneManager.LoadSceneAsync("WeekScene");
    }
}
