using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodReactive : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnMoodChange += OnMoodChange;
    }

    protected virtual void OnMoodChange(Mood newMood) {}
}
