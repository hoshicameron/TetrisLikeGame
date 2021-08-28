using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Transform emptySquare;

    [SerializeField] private int height=30;
    [SerializeField] private int width=10;
    [SerializeField] private int header=8;

    private Transform[,] grid;

    private void Awake()
    {
        grid=new Transform[width,height];
    }

    private void Start()
    {

        DrawEmptyCells();
    }

    private void DrawEmptyCells()
    {
        if (emptySquare == null)
        {
            Debug.Log("Warning! please assign the emptySprite object!");
            return;
        }
        for (int y = 0; y < height-header; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var clone = Instantiate(emptySquare,new Vector3(x,y,0),Quaternion.identity)as Transform;
                clone.name = $"Board Space (x={x.ToString()} , y={y.ToString()})";
                clone.parent = transform;
            }
        }
    }
}
