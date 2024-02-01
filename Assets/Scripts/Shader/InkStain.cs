using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InkStain : MonoBehaviour
{
    
    
    
    public static InkStain CreateStain(float scale = 1f, Vector2 position = new())
    {
        var stain = Instantiate(Resources.Load<GameObject>("Prefabs/InkStain"), position, Quaternion.identity).GetComponent<InkStain>();
        stain.transform.localScale = Vector3.one * scale;
        
        return stain;
    }

    public void StartAnimation(Mood mood, float duration = 1f) => StartCoroutine(PlayAnimation(mood, duration));
    public void StartAnimation(float duration = 1f) => StartCoroutine(PlayAnimation(GameManager.Instance.CurrentMood, duration));

    private IEnumerator PlayAnimation(Mood mood, float duration)
    {
        Material m = GetComponent<SpriteRenderer>().material;
        m.SetColor("_Color1", GameManager.MoodColor[mood].gradiantStart);
        m.SetColor("_Color2", GameManager.MoodColor[mood].gradiantEnd);
        
        
        DOTween.To(x =>
        {
            m.SetFloat(Shader.PropertyToID("_Alpha"), x);
        }, .6f, .01f, 5f);
        yield return DOTween.To(x =>
        {
            m.SetFloat(Shader.PropertyToID("_Size"), x);
        }, -0.6f, .6f, 7f).SetEase(Ease.OutExpo).WaitForCompletion();

        yield return new WaitForSecondsRealtime(duration);
        yield return DOTween.To(x =>
        {
            m.SetFloat(Shader.PropertyToID("_Alpha"), x);
        }, .01f, 0f, 0.5f).WaitForCompletion();
        
        Destroy(gameObject);
    }
}
