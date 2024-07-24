using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq; // Add this to use LINQ methods like First()

public class MapGenerator : MonoBehaviour {
    [SerializeField] private Vector2Int _size;
    [SerializeField] private Vector2 _gap;
    [SerializeField] private MapGenerationData[] _configs;
    [SerializeField] private Grid _grid;

    [SerializeField, Range(0, 0.8f)] private float _skipAmount = 0.1f;
    [SerializeField, Range(0, 1)] private float _forestAmount = 0.3f;

    public MapCameraTarget mapCameraTarget;
    private Vector2 _currentGap;

    private void Start() {
        SpawnRooms();
    }
    private void SpawnRooms() {
        var mapGenerationData = _configs.First();

        // Set grid properties based on configuration
        _grid.cellSize = mapGenerationData.CellSize;
        _grid.cellGap = _currentGap;
        _grid.cellSwizzle = mapGenerationData.GridSwizzle;

        RoomController.instance.roomGrid = new Room[_size.x, _size.y];
        // Initialize coordinates list to store grid positions
        var coordinates = new List<Vector3Int>(); // Changed to Vector2Int if strictly 2D

        // Generate grid coordinates
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                Debug.Log("x: " + x + " y: " + y);
                coordinates.Add(new Vector3Int(x, y)); // Changed to Vector2Int
            }
        }
        

        var bounds = new Bounds();
        var skipCount = Mathf.FloorToInt(coordinates.Count * _skipAmount);
        var forestCount = Mathf.FloorToInt(coordinates.Count * _forestAmount);
        var index = 0;
        // eventually add logic here
        var rand = new System.Random(420);

        foreach (var coordinate in coordinates.OrderBy(t => rand.Next()).Take(coordinates.Count - skipCount))
        {
            var isForest = index++ < forestCount;
            var prefab = isForest ? mapGenerationData.ForestPrefab : mapGenerationData.GrassPrefab;
            var position = _grid.GetCellCenterWorld(coordinate);
            RoomController.instance.LoadRoom("Empty", coordinate.x, coordinate.y, position, prefab);
            bounds.Encapsulate(new Vector3(position.x, 0, position.z));
        }
        Debug.Log("Bounds: " + bounds);
        //calculate the width and height of the final grid area
        var finalGridWidth = _size.x * _grid.cellSize.x;
        var finalGridHeight = _size.y * _grid.cellSize.y; 
        MapCamera.instance.SetCameraTargetPosition(finalGridWidth, finalGridHeight);
        //TODO: add reference to rooms in a list
    }

}