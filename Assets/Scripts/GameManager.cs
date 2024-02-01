using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
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

    public static readonly Dictionary<Mood, (Color gradiantStart, Color gradiantEnd)> MoodColor = new()
    {
        { Mood.HAPPY, (new Color(1f, .8f, 0f), new Color(.75f, .6f, 0f))},
        { Mood.SAD, (new Color(.1f, 0.15f, .9f), new Color(0f, .05f, 0.65f))},
        { Mood.REALLY_SAD, (new Color(0f, 0f, 0.40f), new Color(0f, 0f, .25f))},
        { Mood.LOW_ANGRY, (new Color(1f, .15f, 0.25f), new Color(.63f, 0f, .1f))},
        { Mood.ANGRY, (new Color(.63f, 0f, .1f), new Color(.3f, 0f, .05f))},
        { Mood.BLUSHING, (new Color(.95f, .4f, 1f), new Color(.6f, .25f, .6f))},
        { Mood.IN_LOVE, (new Color(1f, 0f, 0.7f), new Color(.7f, 0f, .5f))},
        { Mood.AWKWARD, (new Color(.15f, .7f, .15f), new Color(.12f, 0.4f, .12f))}
    };

    public List<int> _emotions;

    [SerializeField] private Image _fade;
    private List<(string, string[])> _dialogDatabase = new();
    
    public static GameManager Instance { get; private set; }
    
    public Language GameLanguage { get; private set; } = Language.ENGLISH;
    public List<(string, string[])> DialogDatabase
    {
        get => _dialogDatabase;
    }

    public Day Day { get; private set; } = Day.SUNDAY;
    public Mood CurrentMood { get; private set; }

    public Action<Language> OnGameLanguageChange;
    public Action<Mood> OnMoodChange;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
        Instance = this;
        _emotions = new List<int> { 0, 0, 0, 0 };
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
    
    public IEnumerator CR_MoveObject(GameObject obj, Vector2 newPos)
    {
        SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
        yield return sprite.DOFade(0f, 1f).WaitForCompletion();
        obj.transform.position = newPos;
        yield return sprite.DOFade(1f, 1f).WaitForCompletion();
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

    public void ChangeMood(Mood newMood)
    {
        CurrentMood = newMood;
        OnMoodChange?.Invoke(newMood);
    }

    public void ChangeMood(Mood newMood, Vector2 playerPos)
    {
        InkStain.CreateStain(450f, playerPos).StartAnimation(() => ChangeMood(newMood), newMood);
    }
}
