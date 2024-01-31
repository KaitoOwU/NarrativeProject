using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Subtegral.DialogueSystem.DataContainers;

namespace Subtegral.DialogueSystem.Runtime
{
    public class DialogueParserTest : MonoBehaviour
    {
        [SerializeField] private DialogueContainer dialogue;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Transform buttonContainer;

        [SerializeField] private float choiceTimeout = 10f; // Temps en secondes avant le choix automatique
        private bool isWaitingForChoice = false;

        private Coroutine choiceTimeoutCoroutine;

        private Button _playerButton;
        private Coroutine randomPlayerAnimationCoroutine;

        private void Start()
        {
            _playerButton = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Button>(); ;
            var narrativeData = dialogue.NodeLinks.First(); //Entrypoint node
            ProceedToNarrative(narrativeData.TargetNodeGUID);
        }

        private void ProceedToNarrative(string narrativeDataGUID)
        {
            var text = dialogue.DialogueNodeData.Find(x => x.NodeGUID == narrativeDataGUID).DialogueText;
            var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGUID == narrativeDataGUID);
            dialogueText.text = GameManager.Instance.GetDialog(ProcessProperties(text));
            var buttons = buttonContainer.GetComponentsInChildren<Button>(true);
            //FADE OUT PUIS FADE IN
            var defaultChoice = choices.FirstOrDefault(x => x.PortName == "attendre");
            if(defaultChoice != null)
            {
                StartChoiceTimeout(defaultChoice);
            }
            _playerButton.onClick.RemoveAllListeners();
            var catChoice = choices.FirstOrDefault(x => x.PortName == "Purr");
            if (catChoice != null)
            {
                _playerButton.onClick.AddListener(() => RandomPlayerAnimation(narrativeDataGUID));
            }
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].gameObject.SetActive(false);
                buttons[i].onClick.RemoveAllListeners();
            }
            foreach (var choice in choices)
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    ButtonDatas buttonData = buttons[i].gameObject.GetComponent<ButtonDatas>();
                    Emotions chosenEmotion = buttonData._emotion;
                    if (buttons[i].gameObject.name == choice.PortName)
                    {
                        buttons[i].gameObject.SetActive(true);
                        buttons[i].onClick.AddListener(() =>
                        {
                            StartCoroutine(GameManager.Instance.CR_EndScenario(() =>ChangeSituation(buttonData, chosenEmotion, choice)));
                        });;
                    }
                }
            }
        }

        private void ChangeSituation(ButtonDatas buttonData, Emotions chosenEmotion, NodeLinkData choice)
        {
            StopChoiceTimeout();
            AudioManager.Instance.Play(buttonData._soundName);
            AddEmotion(chosenEmotion);
            ProceedToNarrative(choice.TargetNodeGUID);
        }

        private void AddEmotion(Emotions emotion)
        {
            switch (emotion)
            {
                case Emotions.Positive:
                    GameManager.Instance._emotions[0] ++;
                    break;
                case Emotions.Neutral:
                    GameManager.Instance._emotions[1] ++;
                    break;
                case Emotions.Negative:
                    GameManager.Instance._emotions[2] ++;
                    break;
                case Emotions.NoEmotion:
                    GameManager.Instance._emotions[3] ++;
                    break;
            }
        }

        private string ProcessProperties(string text)
        {
            foreach (var exposedProperty in dialogue.ExposedProperties)
            {
                text = text.Replace($"[{exposedProperty.PropertyName}]", exposedProperty.PropertyValue);
            }
            return text;
        }

        private void StartChoiceTimeout(NodeLinkData defaultChoice)
        {
            isWaitingForChoice = true;
            choiceTimeoutCoroutine = StartCoroutine(ChoiceTimeout(defaultChoice));
        }

        private void StopChoiceTimeout()
        {
            isWaitingForChoice = false;
            if (choiceTimeoutCoroutine != null)
            {
                StopCoroutine(choiceTimeoutCoroutine);
            }
        }

        private IEnumerator ChoiceTimeout(NodeLinkData defaultChoice)
        {
            yield return new WaitForSeconds(choiceTimeout);
            if (isWaitingForChoice)
            {
                if (defaultChoice != null)
                {
                    ProceedToNarrative(defaultChoice.TargetNodeGUID);
                }
            }
        }

        private void RandomPlayerAnimation(string narrativeDataGUID)
        {
            if (randomPlayerAnimationCoroutine == null)
            {
                int randomNumber = UnityEngine.Random.Range(0, 4);
                randomPlayerAnimationCoroutine = StartCoroutine(RandomPlayerAnimationCoroutine(randomNumber, narrativeDataGUID));
            }
        }

        private IEnumerator RandomPlayerAnimationCoroutine(int randomNumber, string narrativeDataGUID)
        {
            var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGUID == narrativeDataGUID);
            switch (randomNumber)
            {
                case 0:
                    GameManager.Instance._emotions[0]++;
                    ProceedToNarrative(choices.FirstOrDefault(x => x.PortName == "Purr").TargetNodeGUID);
                    break;
                case 1:
                    GameManager.Instance._emotions[1]++;
                    ProceedToNarrative(choices.FirstOrDefault(x => x.PortName == "littleMeow").TargetNodeGUID);
                    break;
                case 2:
                    GameManager.Instance._emotions[1]++;
                    ProceedToNarrative(choices.FirstOrDefault(x => x.PortName == "Lick").TargetNodeGUID);
                    break;
                case 3:
                    GameManager.Instance._emotions[2]++;
                    ProceedToNarrative(choices.FirstOrDefault(x => x.PortName == "BigMeow").TargetNodeGUID);
                    break;
            }
            yield return new WaitForSeconds(3f);
            randomPlayerAnimationCoroutine = null;
        }
    }
}