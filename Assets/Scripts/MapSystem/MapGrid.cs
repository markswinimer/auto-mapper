using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class MapGrid : MonoBehaviour
{
    [SerializeField] private Vector2Int _size;
    [SerializeField] private Vector2 _gap;
    [SerializeField, Range(0, 0.8f)] private float _skipAmount = 0.1f;
    [SerializeField, Range(0, 1)] private float _forestAmount = 0.3f;
    [SerializeField] private GridType _gridType;
    [SerializeField] private ScriptableGridConfig[] _configs;
    public MapCameraTarget mapCameraTarget;
    private bool _requiresGeneration = true;
    private Camera _cam;
    private Grid _grid;

    private Vector3 _cameraPositionTarget;
    private float _cameraSizeTarget;
    private Vector3 _moveVel;
    private float _cameraSizeVel;

    private Vector2 _currentGap;
    private Vector2 _gapVel;
    [SerializeField] float southwardOffsetFactor = 0.25f;  // Adjust this value to control how far south the camera targets

    private void Awake()
    {
        _grid = GetComponent<Grid>();
        _cam = Camera.main;
        _currentGap = _gap;
        // _cam.transform.rotation = Quaternion.Euler(45, 0, 0); // Rotates the camera to look straight down
    }

    private void OnValidate() => _requiresGeneration = true;

    private void LateUpdate()
    {
        if (Vector2.Distance(_currentGap, _gap) > 0.01f)
        {
            _currentGap = Vector2.SmoothDamp(_currentGap, _gap, ref _gapVel, 0.1f);
            _requiresGeneration = true;
        }

        if (_requiresGeneration) Generate();



        // moving this to cinemachine
        // TODO: create functions for handling when a level is selected to zoom in
        // this can be used to recenter the camera if the grid changes
        // Smoothly interpolate the camera's position to the target values
        // _cam.transform.position = Vector3.SmoothDamp(_cam.transform.position, _cameraPositionTarget, ref _moveVel, 0.5f);
        // _cam.orthographicSize = Mathf.SmoothDamp(_cam.orthographicSize, _cameraSizeTarget, ref _cameraSizeVel, 0.5f);

        // Ensure the camera is always looking straight down
        // Quaternion targetRotation = Quaternion.Euler(90, 0, 0);
        // _cam.transform.rotation = Quaternion.Lerp(_cam.transform.rotation, targetRotation, Time.deltaTime * 5);
    }

    private void Generate()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        var config = _configs.First(c => c.Type == _gridType);

        _grid.cellLayout = config.Layout;
        _grid.cellSize = config.CellSize;
        if (_grid.cellLayout != GridLayout.CellLayout.Hexagon) _grid.cellGap = _currentGap;
        _grid.cellSwizzle = config.GridSwizzle;

        var coordinates = new List<Vector3Int>();

        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                coordinates.Add(new Vector3Int(x, y));
            }
        }

        var bounds = new Bounds();
        var skipCount = Mathf.FloorToInt(coordinates.Count * _skipAmount);
        var forestCount = Mathf.FloorToInt(coordinates.Count * _forestAmount);
        var index = 0;
        var rand = new Random(420);

        foreach (var coordinate in coordinates.OrderBy(t => rand.Next()).Take(coordinates.Count - skipCount))
        {
            var isForest = index++ < forestCount;
            var prefab = isForest ? config.ForestPrefab : config.GrassPrefab;
            var position = _grid.GetCellCenterWorld(coordinate);
            var spawned = Instantiate(prefab, position, Quaternion.identity, transform);
            spawned.Init(coordinate);
            bounds.Encapsulate(position);
        }

        SetCameraTargetPosition(bounds);

        _requiresGeneration = false;
    }

    private void SetCameraTargetPosition(Bounds bounds)
    {
        // The position of the target the camera should focus on. Meant to view all tiles from a decent point
        _cameraPositionTarget = new Vector3(
            bounds.center.x, //horizontal grid center 
            bounds.max.y + 100, //height above the grid
            bounds.min.z - 0.25f * bounds.size.z); // vertical grid position

        
        Vector3 offsetPosition = bounds.center;

        // Adjust the z position by subtracting a fraction of the bounds' depth
        offsetPosition.z -= southwardOffsetFactor * bounds.size.z;

        // Set the camera target position
        if (mapCameraTarget != null)
        {
            mapCameraTarget.MoveToPosition(bounds.center);
            mapCameraTarget.ScaleToGridSize(bounds);
        }
        else
        {
            Debug.LogError("Camera Target Controller is not assigned!");
        }
    }
}