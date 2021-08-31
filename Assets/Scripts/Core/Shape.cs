using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    [SerializeField] private bool canRotate=true;

    private void Start()
    {

    }

    public void Move(Vector3 moveDirection)
    {
        this.transform.position += moveDirection;
    }

    public void MoveRight()
    {
        Move(Vector3.right);
    }

    public void MoveLeft()
    {
        Move(Vector3.left);
    }

    public void MoveDown()
    {
        Move(Vector3.down);
    }

    public void MoveUp()
    {
        Move(Vector3.up);
    }

    public void RotateRight()
    {
        if(!canRotate)    return;

        transform.Rotate(0f,0f,-90f);
    }

    public void RotateLeft()
    {
        if(!canRotate)    return;

        transform.Rotate(0f,0f,90f);
    }


}
