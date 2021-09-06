using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    [SerializeField] private Vector3 queueOffset;
    [SerializeField] private bool canRotate=true;
    [SerializeField] private string glowSquareTag;
    private GameObject[] glowSquareFXArray;

    public Vector3 QueueOffset => queueOffset;

    private void Awake()
    {
        if (!string.IsNullOrEmpty(glowSquareTag))
        {
            glowSquareFXArray = GameObject.FindGameObjectsWithTag(glowSquareTag);
        }
    }

    public void LandShapeFX()
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            if (glowSquareFXArray[i] != null)
            {
                glowSquareFXArray[i].transform.position = child.position;
                if (glowSquareFXArray[i].TryGetComponent( out ParticlePlayer particlePlayer))
                {
                    particlePlayer.Play();
                }

                i++;
                //ParticlePlayer particlePlayer = glowSquareFXArray[i].GetComponent<ParticlePlayer>();
            }
        }
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

    public void RotateClockwise(bool clockwise)
    {
        if(clockwise)
            RotateRight();
        else
            RotateLeft();
    }
}
