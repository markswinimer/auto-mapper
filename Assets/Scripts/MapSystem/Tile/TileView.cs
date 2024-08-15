using UnityEngine;

public class TileView : MonoBehaviour 
{
    [SerializeField] private GameObject TileVisuals;
    [SerializeField] private GameObject FogOfWar;

    void Awake()
    {
        ToggleTileVisibilty(false);
    }
    
    public void UpdateVisuals(bool visibility)
    {
        // UpdateFogOfWar(visibility);
    }

    public void ToggleTileVisibilty(bool isVisibile)
    {
        TileVisuals.SetActive(isVisibile);
        if (isVisibile)
        {
            // SetLayerAllChildren(TileVisuals, 0);
            FogOfWar.layer = LayerMask.NameToLayer("Hidden");
        }
        else
        {
            FogOfWar.layer = LayerMask.NameToLayer("Default");
        }
    }

    public void AddOutline()
    {
        SetLayerAllChildren(TileVisuals, 7);
    }

    public void RemoveOutline()
    {
        SetLayerAllChildren(TileVisuals, 0);
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
}