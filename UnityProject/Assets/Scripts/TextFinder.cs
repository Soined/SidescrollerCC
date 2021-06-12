using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TextFinder : MonoBehaviour
{
    TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

    }

    private void Update()
    {
        if(Keyboard.current.oKey.wasPressedThisFrame)
        {
            Debug.Log($"page Rank: {text.textInfo.pageInfo[text.pageToDisplay]}");
            Debug.Log($"page pageCount: {text.textInfo.pageCount}");
            Debug.Log($"page pageToDisplay: {text.pageToDisplay}");
        }
    }


}
