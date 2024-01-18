using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogConfig : MonoBehaviour
{
    public List<SpeakerDatabase> speakerDatabases = new();

    [System.Serializable]
    public struct SpeakerConfig
    {
        public enum POSITION
        {
            LEFT,
            MIDDLE,
            RIGHT
        }
        public POSITION position;
        public SpeakerDatabase speakerDatabase;
        public SpeakerData speakerData;

        public void SetPosition(POSITION newPosition)
        {
            this.position = newPosition;
        }
    }

    public List<SpeakerConfig> speakers = new();

    [System.Serializable]
    public struct SentenceConfig
    {
        public string key;
        public AudioClip audioClip;

        public SentenceConfig(string key)
        {
            this.key = key;
            audioClip = null;
        }
    }

    public TextAsset csvDialog;
    public Sentences1Table table;
    [Header("SENTENCES")]
    public List<SentenceConfig> sentenceConfig = new();

}
