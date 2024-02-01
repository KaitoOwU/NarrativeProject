using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    [SerializeField] private Image _triangle;


    private void Start()
    {
        _triangle.transform.DOLocalMoveY(1, 0.5f).OnComplete(() =>
        {
            _triangle.transform.DOLocalMoveY(-1f, 0f);
        }).SetLoops(-1);
    }

    public void ShowTriangle()
    {
        _triangle.DOFade(1f, 0f);
    }
}
