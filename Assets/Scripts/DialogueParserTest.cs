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
        //[SerializeField] private Button choicePrefab;
        [SerializeField] private Transform buttonContainer;

        private void Start()
        {
            var narrativeData = dialogue.NodeLinks.First(); //Entrypoint node
            ProceedToNarrative(narrativeData.TargetNodeGUID);
        }

        private void ProceedToNarrative(string narrativeDataGUID)
        {
            //FADE OUT PUIS FADE IN
            var text = dialogue.DialogueNodeData.Find(x => x.NodeGUID == narrativeDataGUID).DialogueText;
            var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGUID == narrativeDataGUID);
            dialogueText.text = GameManager.Instance.GetDialog(ProcessProperties(text));
            var buttons = buttonContainer.GetComponentsInChildren<Button>(true);
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }

            foreach (var choice in choices)
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    Emotions chosenEmotion = buttons[i].gameObject.GetComponent<ButtonEmotion>()._emotion;
                    if (buttons[i].gameObject.name == choice.PortName)
                    {
                        buttons[i].gameObject.SetActive(true);
                        buttons[i].onClick.AddListener(() =>
                        {
                            AddEmotion(chosenEmotion);
                            ProceedToNarrative(choice.TargetNodeGUID);
                        });
                    }
                }
            }
        }

        private void AddEmotion(Emotions emotion)
        {
            switch (emotion)
            {
                case Emotions.Positive:
                    GameManager.Instance._emotions[0]++;
                    break;
                case Emotions.Neutral:
                    GameManager.Instance._emotions[1]++;
                    break;
                case Emotions.Negative:
                    GameManager.Instance._emotions[2]++;
                    break;
                case Emotions.NoEmotion:
                    GameManager.Instance._emotions[3]++;
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
    }
}