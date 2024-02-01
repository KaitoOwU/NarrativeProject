using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chamber : MoodReactive
{
    [SerializeField] private List<ChamberMood> _roomMoods = new();
    [SerializeField] private Image _plant, _bed, _back;
    
    protected override void OnMoodChange(Mood newMood)
    {
        switch (newMood)
        {
            case Mood.SAD:
                _plant.sprite = _roomMoods[0].plant;
                _bed.sprite = _roomMoods[0].bed;
                _back.sprite = _roomMoods[0].back;
                break;
            
            case Mood.NEUTRAL:
                _plant.sprite = _roomMoods[1].plant;
                _bed.sprite = _roomMoods[1].bed;
                _back.sprite = _roomMoods[1].back;
                break;
            
            case Mood.HAPPY:
                _plant.sprite = _roomMoods[2].plant;
                _bed.sprite = _roomMoods[2].bed;
                _back.sprite = _roomMoods[2].back;
                break;
            
            case Mood.IN_LOVE:
                _plant.sprite = _roomMoods[3].plant;
                _bed.sprite = _roomMoods[3].bed;
                _back.sprite = _roomMoods[3].back;
                break;
        }
    }
    
    [Serializable]
    private struct ChamberMood
    {
        public Sprite plant, bed, back;
    }
}
