using UnityEngine;

[CreateAssetMenu(menuName = "Grid Configuration/Scriptable Grid Config", fileName = "MapGenerationData")]
public class MapGenerationData : ScriptableObject
{
    //we can use this congiguration to eventually create different map pools
    [Space(10)]
    public GridLayout.CellLayout Layout;
    public Room GrassPrefab, ForestPrefab;
    public Vector3 CellSize;
    public GridLayout.CellSwizzle GridSwizzle;
}