using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBox : MonoBehaviour
{
    public TextMeshProUGUI DialogueText;
    public TextMeshProUGUI NameText;

    private Dialogue currentDialogue;

    private int textIndex = 0;
    private int boxIndex = 0;

    public float charDelay = .1f;
    private float _charDelay = 0f;

    private bool isTyping = false;

    private void Update()
    {
        if(isTyping) Typing();
    }

    private void Typing()
    {
        _charDelay -= Time.unscaledDeltaTime;
        if (_charDelay <= 0)
        {
            _charDelay = charDelay;
            DialogueText.text += currentDialogue.boxes[boxIndex].text[textIndex];
            textIndex++;

            if(textIndex == currentDialogue.boxes[boxIndex].text.Length)
            {
                FinishTyping();
            }
        }
    }

    public void SubmitButton()
    {
        if (isTyping) FinishTyping();
        else NextBox();
    }
    void NextBox()
    {
        if (boxIndex >= currentDialogue.boxes.Count - 1)
        {
            DialogueFinished();
            return;
        }
        boxIndex++;
        StartTyping();
    }
    void StartTyping()
    {
        _charDelay = charDelay;
        DialogueText.text = "";
        isTyping = true;
        textIndex = 0;

        NameText.text = currentDialogue.boxes[boxIndex].name;
    }
    void FinishTyping()
    {
        DialogueText.text = currentDialogue.boxes[boxIndex].text;
        isTyping = false;
    }
    void DialogueFinished()
    {
        UIManager.Main.OnDialogueFinished();
    }
    public void StartNewDialogue(Dialogue newDialogue)
    {
        currentDialogue = newDialogue;
        boxIndex = 0;
        StartTyping();
    }
}
