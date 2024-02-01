using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image _fade;
    [SerializeField] private Image _timer;
    public Image Fade => _fade;
    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public IEnumerator CR_Timer(float duration)
    {
        _timer.fillAmount = 1f;
        yield return _timer.DOFade(1f, 0.5f).WaitForCompletion();
        
        yield return _timer.DOFillAmount(0f, duration).WaitForCompletion();
        _timer.DOFade(0f, 1f);
    }
}
