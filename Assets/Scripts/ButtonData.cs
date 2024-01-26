using Subtegral.DialogueSystem.DataContainers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ButtonData", menuName = "My Game/Button Data")]
public class ButtonData : ScriptableObject
{
    public Sprite sprite;
    public float posX;
    public float posY;
    public float height;
    public float width;
}
