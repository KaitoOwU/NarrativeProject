using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog Speaker", menuName = "Dialog/Dialog Speaker")]
public class DialogSpeakerData : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private string _name;
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private DialogSpeakerEmotions _emotions;
    
    public string ID => _id;
    public string Name => _name;
    public Sprite DefaultSprite => _defaultSprite;
    public DialogSpeakerEmotions Emotions => _emotions;
}

[System.Serializable]
public struct DialogSpeakerEmotions
{
    public Sprite neutral, happy, sad, angry;
}
