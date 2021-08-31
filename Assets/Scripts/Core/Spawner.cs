using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Shape[] allShapes;

    private Shape GetRandomShape()
    {
        int index = Random.Range(0, allShapes.Length);
        if (allShapes[index] != null)
        {
            return allShapes[index];
        } else
        {
            Debug.Log("Warning! Shape Invalid.");
            return null;
        }
    }

    public Shape SpawnShape()
    {
        Shape shape = null;
        shape=Instantiate(GetRandomShape(),transform.position,Quaternion.identity) as Shape;
        return shape != null ? shape : null;
    }

    private void Start()
    {

    }
}
