using UnityEngine;

public class Player : MonoBehaviour 
{
    public Vector2Int _currentTile {get; private set; }

    public void Initialize(Vector2Int startPosition) 
    {
        _currentTile = startPosition;
        UpdateWorldMapPosition();
    }

    private void UpdateWorldMapPosition()
    {
        Vector3 worldPosition = Map.Instance.GetPositionFromCoords(_currentTile);
        transform.position = worldPosition;
    }
}