using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Shape[] allShapes;
    [SerializeField] private Transform[] queuedXForms=new Transform[3];
    [SerializeField] private ParticlePlayer spawnFX;


    private Shape[] queuedShapes=new Shape[3];
    private float queueScale = 0.5f;

    private void Awake()
    {
        InitQueue();

    }

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
        shape = GetQueuedShape();
        shape.transform.position = transform.position;
        //shape.transform.localScale = Vector3.one;

        StartCoroutine(GrowShape(shape, transform.position, 0.25f));


        if (spawnFX != null)
        {
            spawnFX.Play();
        }
        return shape != null ? shape : null;
    }

    void InitQueue()
    {
        for (int i = 0; i < queuedShapes.Length; i++)
        {
            queuedShapes[i] = null;
        }
        FillQueue();
    }

    void FillQueue()
    {
        for (int i = 0; i < queuedShapes.Length; i++)
        {
            if (queuedShapes[i]==null)
            {
                queuedShapes[i]=Instantiate(GetRandomShape(),transform.position,Quaternion.identity)as Shape;
                queuedShapes[i].transform.position = queuedXForms[i].position + queuedShapes[i].QueueOffset;
                queuedShapes[i].transform.localScale=new Vector3(queueScale,queueScale,queueScale);

            }
        }
    }

    Shape GetQueuedShape()
    {
        Shape firstShape = null;
        if (queuedShapes[0] != null)
        {
            firstShape = queuedShapes[0];
        }

        for (int i = 1; i < queuedShapes.Length; i++)
        {
            queuedShapes[i - 1] = queuedShapes[i];
            queuedShapes[i - 1].transform.position = queuedXForms[i-1].position + queuedShapes[i].QueueOffset;
        }

        queuedShapes[queuedShapes.Length - 1] = null;
        FillQueue();

        return firstShape;
    }

    IEnumerator GrowShape(Shape shape, Vector3 position, float growTime = 0.5f)
    {

        float size = 0f;
        growTime = Mathf.Clamp(growTime, 0.1f, 2.0f);
        float sizeDelta = Time.deltaTime / growTime;
        while (size<1f)
        {

            shape.transform.localScale=new Vector3(size,size,size);
            size += sizeDelta;
            shape.transform.position = position;
            yield return null;
        }

        shape.transform.localScale = Vector3.one;
    }
}
