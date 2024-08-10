using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MapManager : MonoBehaviour
{
    [SerializeField] private MapCamera mapCamera;
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private string spriteSheetName;
    [SerializeField] private MapGenerator mapGenerator;

    private Map map;
private void Awake()
{
    StartCoroutine(InitializeMap());
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

    ConfigMapCamera(); // This will only be called after the map generation is complete
}

    private void ConfigMapCamera()
    {
        mapCamera.SetCameraTargetPosition(map.MapWorldDimensions);
    }
}
