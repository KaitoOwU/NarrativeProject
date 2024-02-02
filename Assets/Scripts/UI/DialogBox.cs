using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    [SerializeField] private Image _triangle;
    private float _default;


    private void Start()
    {
        _default = _triangle.transform.localPosition.y;
        _triangle.transform.DOLocalMoveY(_default + 20, 1f).OnComplete(() =>
        {
            _triangle.transform.DOLocalMoveY(_default, 0f);
        }).SetLoops(-1);
    }

    public void ShowTriangle()
    {
        _triangle.DOFade(1f, 0f);
    }
}
