using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour
{
    [SerializeField] private Transform holderXForm;
    [SerializeField] private Shape heldShape = null;
    [SerializeField] private bool canRelease = false;
    private float scale = 0.5f;

    public bool CanRelease
    {
        get => canRelease;
        set => canRelease = value;
    }

    public void Catch(Shape shape)
    {
        if (heldShape != null)
        {
            Debug.Log("Holder warning! Release a shape before trying to hold!");
            return;
        }

        if (shape == null)
        {
            Debug.Log("Holder warning! Invalid shape!");
            return;
        }

        if (holderXForm != null)
        {
            shape.transform.position = holderXForm.position + shape.QueueOffset;
            shape.transform.localScale = new Vector3(scale, scale, scale);
            heldShape = shape;
        } else
        {
            Debug.LogWarning("Holder Warning! holder has no transform assigned!");
        }

    }

    public Shape Release()
    {
        if (heldShape == null)
        {
            Debug.Log("Holder warning! Catch a shape before trying to release!");
            return null;
        }

        heldShape.transform.localScale = Vector3.one;
        Shape tempShape = heldShape;
        heldShape = null;
        canRelease = false;
        return tempShape;
    }

    public bool IsHolderEmpty()
    {
        return heldShape == null ? true : false;
    }

}
