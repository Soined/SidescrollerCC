using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void OnInteract();
    void OnCurrent();
    //Will be triggered if the player is not in range of talking to the NPC anymore
    void OnCurrentLost();
}
