using Subtegral.DialogueSystem.DataContainers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    private Button _playerButton;
    private Coroutine randomPlayerAnimationCoroutine;

    private void Awake()
    {
        _playerButton = GetComponent<Button>();
        _playerButton.onClick.AddListener(() => RandomPlayerAnimation());
    }

    private void RandomPlayerAnimation()
    {
        if(randomPlayerAnimationCoroutine == null)
        {
            int randomNumber = Random.Range(0, 3);
            randomPlayerAnimationCoroutine = StartCoroutine(RandomPlayerAnimationCoroutine(randomNumber));
        }
    }

    private IEnumerator RandomPlayerAnimationCoroutine(int randomNumber)
    {
        GameManager.Instance._emotions[randomNumber]++;
        yield return new WaitForSeconds(3f);
        randomPlayerAnimationCoroutine = null;
    }
}
