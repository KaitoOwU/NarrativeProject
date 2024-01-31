using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Emotions
{
    Positive,
    Neutral,
    Negative,
    NoEmotion
}
public class ButtonDatas : MonoBehaviour
{
    public Emotions _emotion;
    public string _soundName;
    public int loveWon;
}
