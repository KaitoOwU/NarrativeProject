using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    [SerializeField] private Image _triangle;

    public void ShowTriangle()
    {
        _triangle.DOFade(1f, 0f);
        Debug.Log("YEs");
    }
}
