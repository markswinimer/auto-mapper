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
    [SerializeField] private Node[,] nodes;
    public MapCameraTarget mapCameraTarget;
    private bool _requiresGeneration = true;
    private Grid _grid;
    private Vector2 _currentGap;
    private Vector2 _gapVel;
    [SerializeField] float southwardOffsetFactor = 0.25f;  // Adjust this value to control how far south the camera targets

    private void Awake()
    {
        _grid = GetComponent<Grid>();
        _currentGap = _gap;
        Generate();
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
        
        // should put camera adjustments here if it needs to be updated
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
            spawned.Init(coordinate, NodeState.Inaccessible);
            // create code to add nodes to Nodes[x,y] array
            bounds.Encapsulate(position);
        }

        // bottom left node is the starting one
        // nodes[0,0].State = NodeState.Accessible;

        SetCameraTargetPosition(bounds);

        _requiresGeneration = false;
    }

    // create function to set which nodes are available

    private void SetCameraTargetPosition(Bounds bounds)
    {
        Vector3 offsetPosition = bounds.center;

        // Adjust the z position by subtracting a fraction of the bounds' depth
        // offsetPosition.z -= southwardOffsetFactor * bounds.size.z;

        // Set the camera target position
        if (mapCameraTarget != null)
        {
            // TODO:why is grid not starting at x = 0?
            // account for the starting x left position of the grid
            // this should be fixed
            float gridOffset = 5f;
            Vector3 modifiedCenter = new Vector3(bounds.center.x + gridOffset, bounds.center.y, bounds.center.z);
            
            mapCameraTarget.MoveToPosition(modifiedCenter);
            mapCameraTarget.ScaleToGridSize(bounds);
        }
        else
        {
            Debug.LogError("Camera Target Controller is not assigned!");
        }
    }
}