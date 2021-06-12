using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextWriter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text tmpro;
    private Text text;

    private string currentText;

    public float charDelay = .1f;
    private float _charDelay = 0f;

    public bool skipWritingOnNextPage = true;

    public bool typeEachPageOnlyOnce = true;

    private bool isTyping = false;
    public bool IsTyping { get => isTyping; }

    private int textIndex = 0; //The location of the character that will be written next
    private int currentPage = 0; //current page we are at
    private bool textFinished = false;
    private bool fastSpeed = false;
    /// <summary>
    /// returns true if the last character in the given text has been written into the box.
    /// </summary>
    public bool TextFinished { get => textFinished; }

    public Action OnWritingFinished;
    /// <summary>
    /// Is called whenever NextPage() is called while being on the last Page of the current text already
    /// </summary>
    public Action OnLastPageNextPage;

    private Dictionary<int, (int firstIndex, int lastIndex)> pageIndexe = new Dictionary<int, (int, int)>();

    private void Update()
    {
        if (isTyping) Typing();
    }
    public void StartWriting(string text)
    {
        currentText = text;
        tmpro.text = "";
        isTyping = true;
        textFinished = false;
        pageIndexe.Clear();
    }
    public void NextPage()
    {
        if (isTyping && skipWritingOnNextPage) //We skip the typing process for current Text
        {
            fastSpeed = true;
            return;
        }

        currentPage++;

        ////If we are done writing, we are not typing anymore, and we are on this page for the first time OR we write every page anyways
        //if (!textFinished && !isTyping && (!typeEachPageOnlyOnce || !pageIndexe.TryGetValue(currentPage, out (int, int) nextPageTuple))) //We start writing the next page
        //{
        //    tmpro.text = "";
        //    isTyping = true;
        //}
        //else if (!textFinished && !isTyping) //We set the text to the next page
        //{
        //    tmpro.text = "";
        //    isTyping = true;
        //    tmpro.text = currentText.Substring(pageIndexe[currentPage].firstIndex, pageIndexe[currentPage].lastIndex - pageIndexe[currentPage].firstIndex);
        //}
        //else if(textFinished)
        //{
        //    OnLastPageNextPage?.Invoke();
        //}
    }
    public void PreviousPage()
    {
        currentPage--;
        if (typeEachPageOnlyOnce)
            tmpro.text = currentText.Substring(pageIndexe[currentPage].firstIndex, pageIndexe[currentPage].lastIndex - pageIndexe[currentPage].firstIndex);
        else
        {
            isTyping = true;
            tmpro.text = "";
            textIndex = pageIndexe[currentPage].firstIndex;
        }
    }
    private void Typing()
    {
        _charDelay -= Time.unscaledDeltaTime;
        if (_charDelay <= 0)
        {
            _charDelay = fastSpeed ? charDelay * 0.01f : charDelay;
            tmpro.text += currentText[textIndex];
            textIndex++;

            if (textIndex == currentText.Length || tmpro.isTextOverflowing)
            {
                Debug.Log($"textIndex: {textIndex}, textLength: {currentText.Length}");
                FinishTyping();
            }
        }
    }

    private void FinishTyping()
    {
        int startIndex = currentPage == 0 ? 0 : pageIndexe[currentPage - 1].lastIndex;
        tmpro.text = GetStringUntilLastWord(currentText.Substring(startIndex, textIndex - startIndex), out int index);
        textIndex = index;

        pageIndexe.Add(currentPage, (startIndex, textIndex));
        isTyping = false;
        fastSpeed = false;


        if (textIndex == currentText.Length)
        {
            Debug.Log($"box done");
            textFinished = true;
            OnWritingFinished?.Invoke();
        }
    }

    private string GetStringUntilLastWord(string text, out int index)
    {
        index = 0;

        for(int i=text.Length - 1; i > 0; i--)
        {
            if (text[i] == ' ' || text[i] == '\n')
            {
                // i ist die Stelle des Leerzeichen, daher fangen wir in der nächsten Box 1 weiter an
                index = i + 1;
                return text.Substring(0, i - 1);
            }
        }
        return text;
    }
}
