using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            UIManager.Main.StartNewDialogue(dialogue);
            gameObject.SetActive(false);
        }
    }
}
