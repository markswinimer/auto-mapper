using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private int[,] gridArray;

    public Grid(int width, int height)
    {
        this.width = width;
        this.height = height;

        gridArray = new int[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Helpers.CreateTextObject(null, gridArray[x, y].ToString(), new Vector3(x, y), 12, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center, 1);
                Debug.Log("Grid created with width: " + width + " and height: " + height);
            }
        }
    }
}
