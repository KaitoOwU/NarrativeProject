using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InkStain : MonoBehaviour
{
    
    
    
    public static InkStain CreateStain(float scale, Vector2 position)
    {
        var stain = Instantiate(Resources.Load<GameObject>("Prefabs/InkStain"), position, Quaternion.identity).GetComponent<InkStain>();
        stain.transform.localScale = Vector3.one * scale;
        return stain;
    }

    public void Start()
    {
        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        Material m = GetComponent<SpriteRenderer>().material;
        DOTween.To(x =>
        {
            m.SetFloat(Shader.PropertyToID("_Size"), x);
        }, -0.6f, .6f, 7f).SetEase(Ease.OutExpo);
        
        DOTween.To(x =>
        {
            m.SetFloat(Shader.PropertyToID("_Alpha"), x);
        }, .6f, .01f, 5f);

        yield break;
    }
}
