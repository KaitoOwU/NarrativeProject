using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InkStain : MonoBehaviour
{
    
    
    
    public static InkStain CreateStain(float scale = 450f, Vector2 position = new())
    {
        var stain = Instantiate(Resources.Load<GameObject>("Prefabs/InkStain"), new Vector3(position.x, position.y, 90), Quaternion.identity).GetComponent<InkStain>();
        stain.transform.localScale = Vector3.one * scale;
        
        return stain;
    }

    public void StartAnimation(Action callback, Mood mood) => StartCoroutine(PlayAnimation(callback, mood));
    public void StartAnimation(Action callback) => StartCoroutine(PlayAnimation(callback, GameManager.Instance.CurrentMood));

    private IEnumerator PlayAnimation(Action callback, Mood mood)
    {
        Material m = GetComponent<SpriteRenderer>().material;
        m.SetColor("_Color1", GameManager.MoodColor[mood].gradiantStart);
        m.SetColor("_Color2", GameManager.MoodColor[mood].gradiantEnd);
        
        DOTween.To(x =>
        {
            m.SetFloat(Shader.PropertyToID("_Size"), x);
        }, -0.6f, .6f, 6f).SetEase(Ease.OutExpo);
        
        yield return new WaitForSecondsRealtime(2.5f);
        callback.Invoke();
        
        yield return DOTween.To(x =>
        {
            m.SetFloat(Shader.PropertyToID("_Alpha"), x);
        }, .9f, 0f, 5f).WaitForCompletion();
        
        Destroy(gameObject);
    }
}
