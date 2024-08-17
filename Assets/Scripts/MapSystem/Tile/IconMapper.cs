using System.Collections.Generic;
using UnityEngine;

public class IconMapper : MonoBehaviour
{
    [SerializeField] private Sprite[] iconSprites;

    private Dictionary<TileType, Sprite> _tileTypeToSprite;

    public static IconMapper Instance;

    void Awake()
    {
        Instance = this;
        InitializeMapping();
    }

    void InitializeMapping()
    {
        print("initing map");
        _tileTypeToSprite = new Dictionary<TileType, Sprite>
        {
            { TileType.Empty, null }, // Assign appropriate sprites
            { TileType.Store, iconSprites[4] },
            { TileType.Battle, iconSprites[2] },
            { TileType.Event, iconSprites[1] },
            { TileType.Final, iconSprites[0] }
            // Add more as needed
        };
    }

    public Sprite GetSpriteForTileType(TileType TileType)
    {
        print("tile: " + TileType);
        if (_tileTypeToSprite.TryGetValue(TileType, out Sprite sprite))
        {
            return sprite;
        }
        else
        {
            Debug.LogWarning($"No sprite found for TileType: {TileType}");
            return null;
        }
    }
}
