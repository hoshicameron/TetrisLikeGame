using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // The empty square Transform referece
    [SerializeField] private Transform emptySquare;
    // The height of board
    [SerializeField] private int height=30;
    // The width of board
    [SerializeField] private int width=10;
    // The clear area at top
    [SerializeField] private int header=8;

    // Reference to Row Glow Effect
    [SerializeField] private ParticlePlayer[]  rowGlowFXArray=new ParticlePlayer[4];

    // An array to store empty square references
    private Transform[,] grid;

    private int completedRows;

    public int CompletedRows => completedRows;

    private void Awake()
    {
        grid=new Transform[width,height];
    }

    private void Start()
    {
        DrawEmptyCells();
    }

    bool IsWithinBoard(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0);
    }

    bool IsOccupied(int x, int y, Shape shape)
    {
        // Return true if this grid cell has object and it's from another shape,Otherwise return false
        return (grid[x, y] != null && grid[x,y].parent!=shape.transform);
    }

    // Is shape in the valid position
    public bool IsValidPosition(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = VectorF.Round(child.position);

            if(!IsWithinBoard((int)pos.x,(int) pos.y))
            {
                return false;
            }
            if(IsOccupied((int)pos.x,(int) pos.y,shape))
            {
                return false;
            }
        }

        return true;
    }
    //Draw empty board with empty sprite object
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
                // Name the empty squares
                clone.name = $"Board Space (x={x.ToString()} , y={y.ToString()})";
                // Parent all the empty square to the board object
                clone.parent = transform;
            }
        }
    }

    public void StoreShapeInGrid(Shape shape)
    {
        if (shape == null)
        {
            return;
        }

        foreach (Transform child in shape.transform)
        {
            Vector2 pos = VectorF.Round(child.position);
            grid[(int) pos.x, (int) pos.y] = child;
        }
    }

    bool IsComplete(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    void ClearRow(int y)
    {

        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                Destroy(grid[x,y].gameObject);
            }

            grid[x, y] = null;
        }


    }

    void ShiftOneRowDown(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y-1].position += Vector3.down;
            }
        }
    }

    void ShiftRowsDown(int startY)
    {
        for (int y = startY; y < height; y++)
        {
            ShiftOneRowDown(y);
        }
    }

    public IEnumerator ClearAllRows()
    {
        completedRows = 0;

        for (int y = 0; y < height; y++)
        {
            if (IsComplete(y))
            {
                ClearRowFX(completedRows,y);
                completedRows++;



            }
        }
        yield return new WaitForSeconds(0.3f);
        for (int y = 0; y < height; y++)
        {
            if (IsComplete(y))
            {
                ClearRow(y);
                ShiftRowsDown(y+1);
                yield return  new WaitForSeconds(0.2f);
                // Decrement counter because we cleared current row, if we continue to next row
                // we will lose one row
                y--;
            }
        }
    }

    public bool IsOverLimit(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            if (child.position.y >= (height - header - 1))
            {
                return true;
            }
        }

        return false;
    }

    private void ClearRowFX(int index,int y)
    {
        if (rowGlowFXArray[index] != null)
        {
            // Set x to effect shown in the top of the blocks
            rowGlowFXArray[index].transform.position=new Vector3(0,y,-1);
            rowGlowFXArray[index].Play();


        }
    }
}
