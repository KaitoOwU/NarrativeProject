using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class DialogueNode : Node
{
    // Start is called before the first frame update
    public string GUID; //unique ID for the node
    public string DialogueText;

    public bool EntryPoint = false; //start point
}
