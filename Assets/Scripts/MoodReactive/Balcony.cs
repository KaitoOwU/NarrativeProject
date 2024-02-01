using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Balcony : MoodReactive
{
    [SerializeField] private List<BalconyMood> _roomMoods = new();
    [SerializeField] private Image _back, _plant1, _grid;

    protected override void OnMoodChange(Mood newMood)
    {
        switch (newMood)
        {
            case Mood.AWKWARD:
                _back.sprite = _roomMoods[0].back;
                _plant1.sprite = _roomMoods[0].plant1;
                _grid.sprite = _roomMoods[0].grid;
                break;

            case Mood.NEUTRAL:
                _back.sprite = _roomMoods[1].back;
                _plant1.sprite = _roomMoods[1].plant1;
                _grid.sprite = _roomMoods[1].grid;
                break;

            case Mood.HAPPY:
                _back.sprite = _roomMoods[2].back;
                _plant1.sprite = _roomMoods[2].plant1;
                _grid.sprite = _roomMoods[2].grid;
                break;

            case Mood.IN_LOVE:
                _back.sprite = _roomMoods[3].back;
                _plant1.sprite = _roomMoods[3].plant1;
                _grid.sprite = _roomMoods[3].grid;
                break;
        }
    }

    [Serializable]
    private struct BalconyMood
    {
        public Sprite back, plant1, plant2, grid;
    }
}
