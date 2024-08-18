using UnityEngine;

public class Player : MonoBehaviour 
{
    public Tile currentTile;
    public Vector2Int currentCoords;
    public bool CanMove {get; private set;}

    public static Player Instance;

    public void Initialize(Tile startingTile) 
    {
        Instance = this;
        CanMove = false;
        currentTile = startingTile;
        currentCoords = currentTile._coordinates;
        UpdateWorldMapPosition(currentCoords);
        this.transform.eulerAngles = new Vector3(0, 150, 0);
        MapManager.OnMapStateChanged += MapManagerOnMapStateChanged;
    }

    void OnDestroy()
    {
        MapManager.OnMapStateChanged -= MapManagerOnMapStateChanged;
    }

    private void MapManagerOnMapStateChanged(MapState state)
    {
        if (state == MapState.MovementPhase)
        {
            CanMove = true;
        }
        else
        {
            CanMove = false;
        }
    }
    
    private void UpdateWorldMapPosition(Vector2Int coordsToMoveTo)
    {
        Vector3 worldPosition = Map.Instance.GetPositionFromCoords(coordsToMoveTo);
        transform.position = worldPosition;
    }

    public void TryMovePlayerToTile(Tile tile)
    {
        //possibly other items to do here, I moved the conditional burden to Tile
        MoveToTile(tile);
    }

    void MoveToTile(Tile tile)
    {
        UpdateWorldMapPosition(tile._coordinates);
        currentCoords = tile._coordinates;
        currentTile = tile;
        Map.Instance.UpdateTileStates(tile._coordinates);
    }
}