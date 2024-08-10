using UnityEngine;

public class TileView : MonoBehaviour 
{
    [SerializeField] private GameObject fogOfWarOverlay;

    public void UpdateVisuals(Tile tileData)
    {
        UpdateFogOfWar(tileData._isRevealed);
    }

    private void UpdateFogOfWar(bool isRevealed)
    {
        fogOfWarOverlay.SetActive(!isRevealed);
    }
}