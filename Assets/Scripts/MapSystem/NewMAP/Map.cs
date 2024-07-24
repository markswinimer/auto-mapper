using UnityEngine;

public class Map : MonoBehaviour {
    public Grid _grid;

    public void Awake()
    {
        _grid = GetComponent<Grid>();
        
    }
    public Vector3 GetPositionFromCoords(Vector3Int coords)
    {
        return _grid.GetCellCenterWorld(coords);
    }
}