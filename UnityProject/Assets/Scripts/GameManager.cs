using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameState GameState
    {
        get => gameState;
    }
    private GameState gameState;


    public static GameManager Main;

    private void Awake()
    {
        if (Main == null) Main = this;
        else if (Main != this) Destroy(this);
    }

    private void Start()
    {
        ChangeGameState(GameState.Playing);
    }

    public void ChangeGameState(GameState newState)
    {
        Time.timeScale = 1;

        switch(newState)
        {
            case GameState.Playing:
                UIManager.Main.ChangeUIState(UIState.HUD);
                break;
            case GameState.Pause:
                UIManager.Main.ChangeUIState(UIState.Pause);
                Time.timeScale = 0;
                break;
            case GameState.Dialogue:
                UIManager.Main.ChangeUIState(UIState.Dialogue);
                Time.timeScale = 0;
                break;
        }
        gameState = newState;
    }

}

public enum GameState
{
    Playing,
    Pause,
    Dialogue
}