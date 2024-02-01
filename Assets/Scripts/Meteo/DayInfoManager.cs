using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DayInfoManager : MonoBehaviour
{
    [SerializeField] private Image _dayImg, _transitionImg;
    [SerializeField] private GameObject _sunObj, _rainyObj;

    private void Start()
    {
        StartCoroutine(CR_Transition());
    }
    
    private IEnumerator CR_Transition()
    {
        switch(GameManager.Instance.GetCurrentDayData().meteo)
        {
            case Meteo.SUNNY:
                _sunObj.SetActive(true);
                break;
            case Meteo.RAINY:
                _rainyObj.SetActive(true);
                break;
        }
        
        yield return _transitionImg.DOFade(0f, 1f).WaitForCompletion();

        yield return new WaitForSecondsRealtime(2.5f);
        
        yield return _transitionImg.DOFade(1f, 1f).WaitForCompletion();
        GameManager.Instance.StartUwu();
    }
}
