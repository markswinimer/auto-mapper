using UnityEngine;

public class Tile : MonoBehaviour, IHoverable
{
    public Vector2Int _coordinates { get; private set; }
    public TileType _tileType { get; private set; }
    public bool _isRevealed { get; private set; }
    public bool canBeHovered { get; private set; } = true;

    private TileView _tileView;

    // Initialization method
    public void Initialize(Vector2Int coords, TileType tileType, GameObject tileViewPrefab)
    {
        _coordinates = coords;
        _tileType = tileType;
        _isRevealed = false;
        Debug.Log(_coordinates);
        // Instantiate the TileView prefab as a child
        GameObject tileViewObject = Instantiate(tileViewPrefab, transform);
        _tileView = tileViewObject.GetComponent<TileView>();

        // Update the visuals based on the initial state
        UpdateVisuals();
    }

    public void RevealTile()
    {
        _isRevealed = true;
        UpdateVisuals();
    }

    public void HideTile()
    {
        _isRevealed = false;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (_tileView != null)
        {
            _tileView.UpdateVisuals(this);
        }
    }

    public void OnHoverEnter()
    {
        _tileView.AddOutline();
    }

    public void OnHoverExit()
    {
        _tileView.RemoveOutline();
    }

    public bool IsHoverable()
    {
        return canBeHovered;
    }
}
