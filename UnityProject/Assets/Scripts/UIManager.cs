using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
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
        Time.timeScale = 10;
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
}
