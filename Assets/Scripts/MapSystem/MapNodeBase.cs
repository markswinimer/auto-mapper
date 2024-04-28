using System;
using UnityEngine;

public class MapNodeBase : MonoBehaviour
{
    private Vector3Int _coordinate;
    private bool _isOccupied;
    private string _nodeType;

    // Initialization method with coordinate and node type
    public void Init(Vector3Int coordinate)
    {
        _coordinate = coordinate;
        // _nodeType = nodeType;
        _isOccupied = false; // Default to not occupied, modify as needed
        // need to rotate the tile to fit the cell shape. 
        // may need to create a better method for doing this.
        transform.rotation = Quaternion.Euler(0, 90f, 0);
    }

    public static implicit operator GameObject(MapNodeBase v)
    {
        throw new NotImplementedException();
    }
}