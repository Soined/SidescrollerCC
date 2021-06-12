using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public Dialogue dialogue;

    public Transform showInteractable;

    [SerializeField] private float cooldown = 0.5f;
    private float _cooldown;

    public bool IsOnCooldown { get => _cooldown > 0; }

    private void Start()
    {
        showInteractable.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (IsOnCooldown) _cooldown -= Time.unscaledDeltaTime;
    }

    public void StartDialogue()
    {
        UIManager.Main.StartNewDialogue(dialogue);
    }
    public void OnInteract()
    {
        if (IsOnCooldown) return;

        StartDialogue();
    }

    public void OnCurrent()
    {
        showInteractable.gameObject.SetActive(true);
        UIManager.Main.OnDialogueFinished += OnDialogueFinished;
    }

    public void OnCurrentLost()
    {
        showInteractable.gameObject.SetActive(false);
        UIManager.Main.OnDialogueFinished -= OnDialogueFinished;
        _cooldown = 0;
    }

    private void OnDialogueFinished()
    {
        StartTimer();
    }

    private void StartTimer()
    {
        _cooldown = cooldown;
    }
}
