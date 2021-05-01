using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbility : ScriptableObject
{
    public bool IsOnCooldown { get => _cooldown > 0f; }

    public float cooldown = 3f;
    private float _cooldown = 0f;
    protected PlayerCharacter character;
    public void Setup(PlayerCharacter character)
    {
        this.character = character;
    }

    public void Update()
    {
        OnAbilityUpdate();

        if (!IsOnCooldown) return;

        _cooldown -= Time.deltaTime;
    }

    public void OnAbilityButtonDown()
    {
        if (IsOnCooldown) return;

        OnAbilityStarted();
    }
    public void OnAbilityButtonUp()
    {
        OnAbilityKeyUp();
    }

    protected void OnAbilityEnded()
    {
        _cooldown = cooldown;
    }

    protected virtual void OnAbilityStarted() { }
    protected virtual void OnAbilityUpdate() { }
    protected virtual void OnAbilityKeyUp() { }
}
