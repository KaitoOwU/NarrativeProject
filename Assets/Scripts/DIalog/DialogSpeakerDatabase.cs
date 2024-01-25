using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog Speaker Database", menuName = "Dialog/Dialog Speaker Database")]
public class DialogSpeakerDatabase : ScriptableObject
{
    [SerializeField] private List<DialogSpeakerData> _speakers = new();
    
    public IReadOnlyList<DialogSpeakerData> Speakers
    {
        get => _speakers;
        set => _speakers = (List<DialogSpeakerData>) value;
    }
}
