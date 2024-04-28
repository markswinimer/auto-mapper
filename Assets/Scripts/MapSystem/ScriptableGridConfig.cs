using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Grid Configuration/Scriptable Grid Config", fileName = "NewGridConfig")]
public class ScriptableGridConfig : ScriptableObject
{
    //we can use this congiguration to eventually create different map pools
    public GridType Type;
    [Space(10)]
    public GridLayout.CellLayout Layout;
    public MapNodeBase GrassPrefab, ForestPrefab;
    public Vector3 CellSize;
    public GridLayout.CellSwizzle GridSwizzle;
}
[Serializable]
public enum GridType
{
    //only using rectangle atm
    Rectangle,
    Isometric,
    HexagonPointy,
    HexagonFlat
}