using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivingRoom : MoodReactive
{

    [SerializeField] private List<LivingRoomMood> _roomMoods = new();
    [SerializeField] private Image _firstPlan, _kitchen, _sofa, _plant1, _plant2, _door;
    private bool _isDoorOpen = false;
    
    protected override void OnMoodChange(Mood newMood)
    {
        switch (newMood)
        {
            case Mood.SAD:
                _firstPlan.sprite = _roomMoods[0].firstPlan;
                _kitchen.sprite = _roomMoods[0].kitchen;
                _sofa.sprite = _roomMoods[0].sofa;
                _plant1.sprite = _roomMoods[0].plant1;
                _plant2.sprite = _roomMoods[0].plant2;
                _door.sprite = _isDoorOpen ? _roomMoods[0].opened : _roomMoods[0].closed;
                break;
            
            case Mood.NEUTRAL:
                _firstPlan.sprite = _roomMoods[1].firstPlan;
                _kitchen.sprite = _roomMoods[1].kitchen;
                _sofa.sprite = _roomMoods[1].sofa;
                _plant1.sprite = _roomMoods[1].plant1;
                _plant2.sprite = _roomMoods[1].plant2;
                _door.sprite = _isDoorOpen ? _roomMoods[1].opened : _roomMoods[1].closed;
                break;
            
            case Mood.HAPPY:
                _firstPlan.sprite = _roomMoods[2].firstPlan;
                _kitchen.sprite = _roomMoods[2].kitchen;
                _sofa.sprite = _roomMoods[2].sofa;
                _plant1.sprite = _roomMoods[2].plant1;
                _plant2.sprite = _roomMoods[2].plant2;
                _door.sprite = _isDoorOpen ? _roomMoods[2].opened : _roomMoods[2].closed;
                break;
            
            case Mood.IN_LOVE:
                _firstPlan.sprite = _roomMoods[3].firstPlan;
                _kitchen.sprite = _roomMoods[3].kitchen;
                _sofa.sprite = _roomMoods[3].sofa;
                _plant1.sprite = _roomMoods[3].plant1;
                _plant2.sprite = _roomMoods[3].plant2;
                _door.sprite = _isDoorOpen ? _roomMoods[3].opened : _roomMoods[3].closed;
                break;
        }
    }

    public void SetDoorOpen(bool open)
    {
        _isDoorOpen = open;
        OnMoodChange(GameManager.Instance.CurrentMood);
    }
    

    [Serializable]
    private struct LivingRoomMood
    {
        public Sprite firstPlan, kitchen, sofa, plant1, plant2, opened, closed;
    }
}
