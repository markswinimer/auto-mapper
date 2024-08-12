using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;   

    public GameState State; 

    public static event Action<GameState> OnGameStateChanged;

    void Awake() {
        Instance = this;
    }

    void Start()
    {
        UpdateGameState(GameState.LoadMap);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.LoadMap:
                break;
            case GameState.Movement:
                break;
            case GameState.OnTile:
                break;
            case GameState.BattleTransition:
                break;
            case GameState.MapTransition:
                break;
            case GameState.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }
}

public enum GameState
{
    LoadMap,
    Movement,
    OnTile,
    BattleTransition,
    MapTransition,
    Lose,
}