using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_DayName : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _animator.Play(GameManager.DayName[GameManager.Instance.Day] + "Anim");
    }
}
