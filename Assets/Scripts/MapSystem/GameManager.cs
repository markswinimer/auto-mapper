using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// this file has lots of dummy variables to illustrate functionality
public class GameManager : MonoBehaviour {
    public static GameManager Instance;   

    public GameState State; 
    private string _saveData;
    private bool _isNewMap;
    private string _viewType;

    public static event Action<GameState> OnGameStateChanged;

    void Awake() {
        Instance = this;
    }

    void Start()
    {
        UpdateGameState(GameState.LoadingGameState);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.LoadingGameState:
                // adding this psuedo function just for poc
                PrepareGame();

                // this is where we load dependencies,
                // load game data and state and make sure we are good to begin a game
                // then update state to map or battle view depending on loaded data (or default map view)
                break;
            case GameState.InitMapView:
                
                // we end up with some variable from save data like
                // this should be a new map
                // this is an existingmap
                MapManager.Instance.InitMapView(_isNewMap);

                break;
            case GameState.ReturnToMapView:
                MapManager.Instance.ReturnToMapView();

                break;
            case GameState.BattleView:
                // BattleManager.Instance.InitBattle();

                // Mark to Ryan: Add the init function for your battle code
                // BattleManager could be set up like my MapManager for instance
                
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    void PrepareGame()
    {
        LoadSaveData();
        // DoOtherThings();

        // do something with save data and interpret it
        // if we were on a map load the map view
        // if we need to make a map, load the map view
        // if we are in a battle, load the battle view 
        
        if ( _viewType == "map" ) { 
            UpdateGameState(GameState.InitMapView);
        } 
        else if ( _viewType == "battle" )
        { 
            UpdateGameState(GameState.BattleView);
        } else { 
            // we done fucked up 
        }
    }

    void LoadSaveData()
    {
        _saveData = "savedata";
        _isNewMap = true;
        _viewType = "map";
        // load the data or somehting
        // has its own script for this, maybe ienum
    }
}


public enum GameState
{
    LoadingGameState,
    InitMapView,
    ReturnToMapView,
    MapView,
    BattleView
    // addition states could be like pause screen
}