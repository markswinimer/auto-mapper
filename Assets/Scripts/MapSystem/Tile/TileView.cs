using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.U2D;

public class TileView : MonoBehaviour 
{
    [SerializeField] private GameObject _tileVisuals;
    [SerializeField] private GameObject _fogOfWar;
    [SerializeField] private GameObject _tileCanvas;

    private GameObject _icon;
    private Vector3 _iconInitialScale;
    private float _iconScaleMultiplier = 2f;

    private Renderer renderer;

    private bool IsRevealed = false;

    void Awake()
    {
        renderer = _fogOfWar.GetComponent<Renderer>();
        ToggleTileVisibilty(false);
    }
    
    public void UpdateVisuals(bool visibility)
    {
        // Update_fogOfWar(visibility);
    }
    
    void Modify_fogOfWar()
    {
        Color color = renderer.material.color;

        // Set the alpha to 0.5 (50% transparency)
        color.a = 0.2f;

        // Apply the updated color back to the material
        renderer.material.color = color;
    }

    public void ToggleTileVisibilty(bool isVisibile)
    {
        if (isVisibile)
        {
            if (!IsRevealed)
            {
                _tileVisuals.SetActive(true);
                IsRevealed = true;
                Modify_fogOfWar();
                _fogOfWar.layer = LayerMask.NameToLayer("Hidden");
            }
            else {
                _fogOfWar.layer = LayerMask.NameToLayer("Hidden");
            }
        }
        else
        {
            if (IsRevealed)
            {
                _fogOfWar.layer = LayerMask.NameToLayer("Default");
            }
            else
            {
                _fogOfWar.layer = LayerMask.NameToLayer("Default");
                _tileVisuals.SetActive(false);
            }
        }
    }

    public void AddOutline()
    {
        SetLayerAllChildren(_tileVisuals, 7);
    }

    public void RemoveOutline()
    {
        SetLayerAllChildren(_tileVisuals, 0);
    }

    void SetLayerAllChildren(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        var children = gameObject.transform.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            child.gameObject.layer = layer;
        }
    }

    public void ScaleIcon()
    {
        _icon.transform.localScale = _iconInitialScale * _iconScaleMultiplier;
    }
    
    public void DescaleIcon()
    {
        _icon.transform.localScale = _iconInitialScale;
    }
    public void SetIcon(Sprite sprite)
    {
        print(sprite);

        if (sprite)
        {

            GameObject iconObject = new GameObject("Icon");

            _icon = iconObject;

            // Add a SpriteRenderer component to the new GameObject
            SpriteRenderer spriteRenderer =  iconObject.AddComponent<SpriteRenderer>();

            // Assign the sprite to the SpriteRenderer
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingLayerName = "UI";
            spriteRenderer.sortingOrder = 10;


            // Set the new GameObject as a child of the emptyGameObject
            iconObject.transform.SetParent(_tileCanvas.transform);
            _iconInitialScale = _icon.transform.localScale * 2f;


            iconObject.transform.localPosition = new Vector3(0f, 0f, 0f);
            iconObject.transform.localScale = _iconInitialScale;
        }
        else
        {
            Debug.LogWarning("Empty GameObject or Icon Sprite is not assigned!");
        }
    }
}