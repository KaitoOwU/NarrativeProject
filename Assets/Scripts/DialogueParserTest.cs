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
            var defaultChoice = choices.FirstOrDefault(x => x.PortName == "attendre");
            if (defaultChoice != null)
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
                if (buttons[i].gameObject.name == "monologuePrefab")
                {
                    buttons[i].gameObject.SetActive(false);
                }
                buttons[i].onClick.RemoveAllListeners();
            }

            if(choices.ToList().Count == 0)
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (buttons[i].gameObject.name == "monologuePrefab")
                    {
                        buttons[i].gameObject.SetActive(true);
                        buttons[i].onClick.AddListener(() =>
                        {
                            StartCoroutine(GameManager.Instance.CR_EndDay());
                        }); ;
                    }
                }
                return;
            }

            if(choices.ToList().Count == 1 && choices.ToList()[0].PortName == "changeSituation")
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (buttons[i].gameObject.name == "monologuePrefab")
                    {
                        buttons[i].gameObject.SetActive(true);
                        buttons[i].onClick.AddListener(() =>
                        {
                            StartCoroutine(GameManager.Instance.CR_EndScenario(() => ChangeSituationMonologue("sonBanger", Emotions.NoEmotion, choices.ToList()[0])));
                            return;
                        });
                    }
                }
            }


            foreach (var choice in choices)
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    ButtonDatas buttonData = buttons[i].gameObject.GetComponentInChildren<ButtonDatas>();
                    Emotions chosenEmotion = buttonData._emotion;
                    if (buttons[i].gameObject.name == choice.PortName)
                    {
                        if (buttons[i].gameObject.name != "monologuePrefab")
                        {
                            buttons[i].onClick.AddListener(() =>
                            {
                                ChangeSituation(buttonData._soundName, chosenEmotion, choice);
                                //StartCoroutine(GameManager.Instance.CR_EndScenario(() => ChangeSituation(buttonData._soundName, chosenEmotion, choice)));
                            });
                            Debug.Log("Listener added on " + buttons[i].gameObject.name);
                        }
                        else
                        {
                            buttons[i].gameObject.SetActive(true);
                            buttons[i].onClick.AddListener(() =>
                            {
                                ChangeSituationMonologue(buttonData._soundName, chosenEmotion, choice);
                            });
                        }
                    }
                }
            }
        }

        private Mood TranslateMood(Emotions emotion)
        {
            switch(emotion)
            {
                case Emotions.Positive:
                    return Mood.HAPPY;
                case Emotions.Negative:
                    return Mood.SAD;
                case Emotions.Neutral:
                    return Mood.NEUTRAL;
            }
            return Mood.NEUTRAL;
        }

        private void ChangeSituation(string soundName, Emotions chosenEmotion, NodeLinkData choice)
        {
            Vector2 playerPos = new Vector2(0, 0);
            GameManager.Instance.ChangeMood(TranslateMood(chosenEmotion), playerPos);
            StopChoiceTimeout();
            AudioManager.Instance.Play(soundName);
            AddEmotion(chosenEmotion);
            ProceedToNarrative(choice.TargetNodeGUID);
        }

        private void ChangeSituationMonologue(string soundName, Emotions chosenEmotion, NodeLinkData choice)
        {
            StopChoiceTimeout();
            AudioManager.Instance.Play(soundName);
            AddEmotion(chosenEmotion);
            ProceedToNarrative(choice.TargetNodeGUID);
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
            int randomNumber = UnityEngine.Random.Range(0, 4);
            var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGUID == narrativeDataGUID);
            switch (randomNumber)
            {
                case 0:
                    ChangeSituation("Purr", Emotions.Positive, choices.FirstOrDefault(x => x.PortName == "Purr"));
                    //StartCoroutine(GameManager.Instance.CR_EndScenario(() => ChangeSituation("Purr", Emotions.Positive, choices.FirstOrDefault(x => x.PortName == "Purr"))));
                    break;
                case 1:
                    ChangeSituation("LittleMeow", Emotions.Neutral, choices.FirstOrDefault(x => x.PortName == "LittleMeow"));
                    //StartCoroutine(GameManager.Instance.CR_EndScenario(() => ChangeSituation("LittleMeow", Emotions.Neutral, choices.FirstOrDefault(x => x.PortName == "LittleMeow"))));
                    break;
                case 2:
                    ChangeSituation("Lick", Emotions.Neutral, choices.FirstOrDefault(x => x.PortName == "Lick"));
                    //StartCoroutine(GameManager.Instance.CR_EndScenario(() => ChangeSituation("Lick", Emotions.Neutral, choices.FirstOrDefault(x => x.PortName == "Lick"))));
                    break;
                case 3:
                    ChangeSituation("BigMeow", Emotions.Negative, choices.FirstOrDefault(x => x.PortName == "BigMeow"));
                    //StartCoroutine(GameManager.Instance.CR_EndScenario(() => ChangeSituation("BigMeow", Emotions.Negative, choices.FirstOrDefault(x => x.PortName == "BigMeow"))));
                    break;
            }
        }
    }
}