using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private Color color=new Color(1f,1f,1f,0.2f);
    private Shape ghostShape;
    private bool hitBottom;

    public void DrawGhost(Shape originalShape,Board gameBoard)
    {
        if (ghostShape == null)
        {
            ghostShape=Instantiate(originalShape,originalShape.transform.position,originalShape.transform.rotation) as Shape;

            ghostShape.name = "GhostShape";

            SpriteRenderer[] allRenderers = ghostShape.GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer rend in allRenderers)
            {
                rend.color = color;
            }
        } else
        {
            ghostShape.transform.position = originalShape.transform.position;
            ghostShape.transform.rotation = originalShape.transform.rotation;
        }

        hitBottom = false;
        while (!hitBottom)
        {
            ghostShape.MoveDown();
            if (!gameBoard.IsValidPosition(ghostShape))
            {
                ghostShape.MoveUp();
                hitBottom = true;
            }
        }
    }

    public void Reset()
    {
        Destroy(ghostShape.gameObject);
    }
}
