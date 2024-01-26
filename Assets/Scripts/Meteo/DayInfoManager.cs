using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayInfoManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Image _dayImg, _transitionImg;
    [SerializeField] private GameObject _sunObj, _cloudyObj, _rainyObj;

    private void Start()
    {
        StartCoroutine(CR_Transition());
    }
    
    private IEnumerator CR_Transition()
    {
        _description.DOText(GameManager.Instance.GetDialog($"DAY{(int)GameManager.Instance.Day}_DESC"), 1f);
        yield return _transitionImg.DOFade(0f, 1f).WaitForCompletion();

        yield return new WaitForSecondsRealtime(5f);
        
        yield return _transitionImg.DOFade(1f, 1f).WaitForCompletion();
    }
}
