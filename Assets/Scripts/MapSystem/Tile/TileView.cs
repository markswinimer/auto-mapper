using UnityEngine;

public class TileView : MonoBehaviour 
{
    [SerializeField] private GameObject fogOfWarOverlay;

    public void UpdateVisuals(bool visibility)
    {
        UpdateFogOfWar(visibility);
    }

    private void UpdateFogOfWar(bool isRevealed)
    {
        fogOfWarOverlay.SetActive(!isRevealed);
    }

    public void AddOutline()
    {
        SetLayerAllChildren(gameObject.transform, 7);
    }

    public void RemoveOutline()
    {
        SetLayerAllChildren(gameObject.transform, 0);
    }

    void SetLayerAllChildren(Transform root, int layer)
    {
        gameObject.layer = layer;
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            child.gameObject.layer = layer;
        }
    }
}