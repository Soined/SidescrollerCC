using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBox : MonoBehaviour
{
    public TextMeshProUGUI DialogueText;

    public string dialogueString;
    private int dialogueIndex = 0;

    public float charDelay = .1f;
    private float _charDelay = 0f;

    private bool isTyping = false;

    private void Start()
    {
        StartTyping();
    }

    private void Update()
    {
        if(isTyping) Typing();
    }

    private void Typing()
    {
        _charDelay -= Time.deltaTime;
        if (_charDelay <= 0)
        {
            _charDelay = charDelay;
            DialogueText.text += dialogueString[dialogueIndex];
            dialogueIndex++;

            if(dialogueIndex == dialogueString.Length)
            {
                FinishTyping();
            }
        }
    }
    public void StartTyping()
    {
        _charDelay = charDelay;
        DialogueText.text = "";
        isTyping = true;
    }
    public void FinishTyping()
    {
        DialogueText.text = dialogueString;
        isTyping = false;
    }
}
