using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBox : MonoBehaviour
{
    public TextMeshProUGUI NameText;

    private Dialogue currentDialogue;

    private int boxIndex = 0;

    public TextWriter writer;



    public void SubmitButton()
    {
        if(writer.TextFinished) NextBox();
        else writer.NextPage();
    }
    void NextBox()
    {
        if (boxIndex >= currentDialogue.boxes.Count - 1)
        {
            DialogueFinished();
            return;
        }
        boxIndex++;
        writer.StartWriting(currentDialogue.boxes[boxIndex].text);
    }
    void DialogueFinished()
    {
        UIManager.Main.DialogueFinished();
    }
    public void StartNewDialogue(Dialogue newDialogue)
    {
        boxIndex = 0;
        currentDialogue = newDialogue;
        writer.StartWriting(currentDialogue.boxes[boxIndex].text);
    }
}
