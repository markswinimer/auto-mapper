using System.Numerics;
using UnityEngine;

public class Tile : MonoBehaviour, IInteractable
{
    public Vector2Int _coordinates { get; private set; }
    public TileType _tileType { get; private set; }

    public bool CanVisit { get; set; } = false;
    public bool IsRevealed { get; set; } = false;

    public bool _isHoverable { get; private set; } = true;
    public bool _isClickable { get; private set; } = false;

    public TileAccess tileAccess;

    private TileView _tileView;

    void Awake()
    {
        tileAccess = TileAccess.CanAccess;
        _isHoverable = true;
        _isClickable = true;
    }
    // Initialization method
    public void Initialize(Vector2Int coords, TileType tileType, GameObject tileViewPrefab)
    {
        _coordinates = coords;
        _tileType = tileType;
        Debug.Log(_coordinates);
        // Instantiate the TileView prefab as a child
        GameObject tileViewObject = Instantiate(tileViewPrefab, transform);
        _tileView = tileViewObject.GetComponent<TileView>();

        // Update the visuals based on the initial state
        UpdateVisuals();

        Map.Instance.AddTile(coords, this);
    }

    public void ToggleTileVisibilty(bool isVisible)
    {
        _tileView.ToggleTileVisibilty(isVisible);
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
        return _isHoverable;
    }

    public void OnClick()
    {
        if (CanVisit && tileAccess == TileAccess.CanAccess)
        {
            Player.Instance.TryMovePlayerToTile(this);
        }
    }

    public bool IsClickable()
    {
        return _isClickable;
    }
}