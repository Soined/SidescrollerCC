using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue", order = 0)]
public class Dialogue : ScriptableObject
{
    public List<DialogueBox> boxes = new List<DialogueBox>();
}

[System.Serializable]
public struct DialogueBox
{
    public string name;
    [TextArea]
    public string text;
}
