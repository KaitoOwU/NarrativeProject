using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

public class InkStain : MonoBehaviour
{
    
    
    
    public static InkStain CreateStain(float scale = 450f, Vector2 position = new())
    {
        var stain = Instantiate(Resources.Load<GameObject>("Prefabs/InkStain"), new Vector3(position.x, position.y, 90), Quaternion.identity).GetComponent<InkStain>();
        stain.transform.localScale = Vector3.one * scale;
        
        return stain;
    }

    public void StartMoodChangeAnimation([CanBeNull] Action callback, Mood mood) => StartCoroutine(PlayMoodChangeAnimation(callback, mood));
    public void StartMoodChangeAnimation([CanBeNull] Action callback) => StartCoroutine(PlayMoodChangeAnimation(callback, GameManager.Instance.CurrentMood));

    private IEnumerator PlayMoodChangeAnimation(Action callback, Mood mood)
    {
        Material m = GetComponent<SpriteRenderer>().material;
        m.SetColor("_Color1", GameManager.MoodColor[mood].gradiantStart);
        m.SetColor("_Color2", GameManager.MoodColor[mood].gradiantEnd);
        
        Destroy(gameObject, 7.5f);
        
        DOTween.To(x =>
        {
            m.SetFloat(Shader.PropertyToID("_Size"), x);
        }, -0.6f, .6f, 6f).SetEase(Ease.OutExpo);
        
        yield return new WaitForSecondsRealtime(2.5f);
        callback?.Invoke();
        
        yield return DOTween.To(x =>
        {
            m.SetFloat(Shader.PropertyToID("_Alpha"), x);
        }, .9f, 0f, 5f).WaitForCompletion();
    }

    public void StartAuraAnimation(Mood mood) => StartCoroutine(PlayAuraAnimation(mood));

    public IEnumerator PlayAuraAnimation(Mood mood)
    {
        Material m = GetComponent<SpriteRenderer>().material;
        m.SetColor("_Color1", GameManager.MoodColor[mood].gradiantStart);
        m.SetColor("_Color2", GameManager.MoodColor[mood].gradiantEnd);
        
        DOTween.To(x =>
        {
            m.SetFloat(Shader.PropertyToID("_Size"), x);
        }, -0.6f, .6f, 6f).SetEase(Ease.OutExpo);
        yield break;
    }
}
