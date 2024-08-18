using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    public MapState State;

    [SerializeField] private MapCamera mapCamera;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private string spriteSheetName;
    [SerializeField] private MapGenerator mapGenerator;

    public static event Action<MapState> OnMapStateChanged;

    private GameObject _player;
    private Map map;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateMapState(MapState newState)
    {
        State = newState;

        switch (newState)
        {
            case MapState.InitMap:
                InitializeMap();
                
                break;
            case MapState.MapInstantiated:
                FinishMapSetup();
                
                break;
            case MapState.ResumeMap:
                ResumeMap();

                break;
            case MapState.MovementPhase:
                break;
            case MapState.TilePhase:
                break;
            case MapState.MenuPhase:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnMapStateChanged?.Invoke(newState);
    }

    // I'm thinking this is the right strategy, data will be loaded and context passed here 
    // from gamemanager?
    public void InitMapView(bool isNewMap)
    {
        if (isNewMap)
        {
            UpdateMapState(MapState.InitMap);
        }
        else
        {
            UpdateMapState(MapState.ResumeMap);
        }
    }

    // I think all map generation and loading may need to be in its own script
    void InitializeMap()
    {
        if (Map.Instance == null)
        {
            mapGenerator.GenerateMap();
            Debug.Log("generate map");
        }
        else
        {
            UpdateMapState(MapState.MapInstantiated);
            Debug.Log("map found");
        }
    }

    void FinishMapSetup()
    {
        map = Map.Instance;
        CreatePlayerOnMap();
        ConfigMapCamera();
        UpdateMapState(MapState.MovementPhase);
    }

    void ResumeMap()
    {
        //
    }

    public void ReturnToMapView()
    {

    }

    private void ConfigMapCamera()
    {
        mapCamera.SetCameraTargetPosition(map.MapWorldDimensions);
    }

    private void CreatePlayerOnMap()
    {
        // start position will eventually be loaded from save and should be a variable, otherwise bottom left
        Vector2Int startPosition = new Vector2Int(0,0);
        Tile startingTile = Map.Instance.GetTileAtCoordinate(startPosition);
        // Instantiate the player at the start position
        GameObject playerInstance = Instantiate(playerPrefab);

        // Get the Player script component and initialize it
        Player playerComponent = playerInstance.GetComponent<Player>();
        _player = playerInstance;

        // MapCamera.instance.SetCameraFollow(_player.transform);
        if (playerComponent != null)
        {
            Map.Instance.UpdateTileStates(startPosition);
            playerComponent.Initialize(startingTile);
        }
        else
        {
            Debug.LogError("Player component not found on playerPrefab.");
        }
    }
}

public enum MapState {
    InitMap,
    LoadMap,
    GenerateMap,
    MapInstantiated,
    ResumeMap,
    MovementPhase,
    TilePhase,
    MenuPhase
}