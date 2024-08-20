using UnityEngine;

public class Tile : MonoBehaviour, IInteractable
{
    public Vector2Int _coordinates { get; private set; }
    public TileType _tileType { get; private set; }

    public bool CanVisit { get; set; } = false;
    public bool IsRevealed { get; set; } = false;

    public bool _isHoverable { get; private set; } = true;
    public bool _isClickable { get; private set; } = false;

    // ui test
    private float _targetScale = 1;
    private Vector3 _scaleVelocity;
    private Quaternion _targetRotation;

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

    void Update()
    {
        transform.localScale =
          Vector3.SmoothDamp(transform.localScale,
              _targetScale * Vector3.one, ref _scaleVelocity, .15f);

        transform.rotation = Quaternion.Slerp(transform.rotation,
            _targetRotation, Time.deltaTime * 5);
    }

    private void OnEnable() 
    {
        MainUI.ScaleChanged += OnScaleChanged;
        MainUI.SpinClicked += MainUIOnSpinClicked;
    }

    private void OnDisable() 
    {
        MainUI.ScaleChanged -= OnScaleChanged;
        MainUI.SpinClicked -= MainUIOnSpinClicked;
    }

    void OnScaleChanged(float newScale)
    {
        _targetScale = newScale;
    }
    void MainUIOnSpinClicked()
    {
        _targetRotation = transform.rotation * Quaternion.Euler(Random.insideUnitSphere * 360);
    }
    public void ToggleTileVisibilty(bool isVisible)
    {
        if ( IsRevealed )
        {
            
        }
        _tileView.ToggleTileVisibilty(isVisible);
    }

    private void UpdateVisuals()
    {

        if (_tileView != null)
        {
            SetTileIcon();
        }
    }

    public void OnHoverEnter()
    {
        _tileView.AddOutline();
        _tileView.ScaleIcon();
    }

    public void OnHoverExit()
    {
        _tileView.RemoveOutline();
        _tileView.DescaleIcon();
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

    public void SetTileIcon()
    {   
        print("set icon");
        Sprite tileIcon = IconMapper.Instance.GetSpriteForTileType(_tileType);
        print(tileIcon);
        _tileView.SetIcon(tileIcon);
    }
}