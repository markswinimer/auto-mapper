using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MapManager : MonoBehaviour
{
    [SerializeField] private MapCamera mapCamera;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private string spriteSheetName;
    [SerializeField] private MapGenerator mapGenerator;

    private Map map;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        StartCoroutine(InitializeMap());
    }

    void OnDestroy() 
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    void GameManagerOnGameStateChanged(GameState state)
    {

    }

    private IEnumerator InitializeMap()
    {
        Map existingMap = FindObjectOfType<Map>();

        if (existingMap == null)
        {
            Debug.Log("generate map");
            yield return StartCoroutine(mapGenerator.GenerateMap()); // Wait until map generation is complete
            map = FindObjectOfType<Map>(); // Get the generated map reference
        }
        else
        {
                Debug.Log("map found");

                // Assuming LoadMap() might also take some time, if so, use another coroutine
                // yield return StartCoroutine(mapLoader.LoadMap()); // Uncomment if mapLoader is asynchronous
                map = existingMap;
        }
        CreatePlayerOnMap();
        ConfigMapCamera(); // This will only be called after the map generation is complete
    }

    private void ConfigMapCamera()
    {
        mapCamera.SetCameraTargetPosition(map.MapWorldDimensions);
    }

    private void CreatePlayerOnMap()
    {
        // start position will eventually be loaded from save and should be a variable, otherwise bottom left
        Vector2Int startPosition = new Vector2Int(0,0);

        // Instantiate the player at the start position
        GameObject playerInstance = Instantiate(playerPrefab);

        // Get the Player script component and initialize it
        Player playerComponent = playerInstance.GetComponent<Player>();

        if (playerComponent != null)
        {
            playerComponent.Initialize(startPosition);
        }
        else
        {
            Debug.LogError("Player component not found on playerPrefab.");
        }
    }
}