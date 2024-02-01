using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Image _play;
    
    public void Press()
    {
        _play.transform.DOScale(.9f, .3f);
    }
    
    public void Unpress()
    {
        _play.transform.DOScale(1f, .3f);
    }
}
