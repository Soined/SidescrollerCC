using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private UIState state;

    [SerializeField] private GameObject HUDpanel, pausePanel, dialoguePanel;

    public static UIManager Main;

    [SerializeField] private TextMeshProUGUI TimerText;

    private float currentTime = 0;

    private void Awake()
    {
        if (Main == null)
        {
            Main = this;
        }
        else if (Main != this)
        {
            Destroy(this);
        }
    }


    private void Start()
    {
        TimerText.text = "test";
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        int currentTimeInt = (int)Mathf.Ceil(currentTime);

        int currentSeconds = currentTimeInt % 60;
        int currentMinutes = currentTimeInt / 60;

        TimerText.text = $"{currentMinutes.GetAsDoubleDigit()}:{currentSeconds.GetAsDoubleDigit()}";


        //Timer, der entsprechend bei 00:00 hochzählt.
    }

    public void ChangeUIState(UIState newState)
    {
        DeactivateAllPanels();
        switch(newState)
        {
            case UIState.HUD:
                HUDpanel.SetActive(true);
                break;
            case UIState.Pause:
                pausePanel.SetActive(true);
                break;
            case UIState.Dialogue:
                dialoguePanel.SetActive(true);
                break;
        }
        state = newState;
    }
    private void DeactivateAllPanels()
    {
        HUDpanel.SetActive(false);
        pausePanel.SetActive(false);
        dialoguePanel.SetActive(false);
    }
}

public enum UIState
{
    HUD,
    Pause,
    Dialogue
}
