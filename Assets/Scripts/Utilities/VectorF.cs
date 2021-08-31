using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorF
{
    public static Vector2 Round(Vector2 vector2)
    {
        return new Vector2(Mathf.Round(vector2.x),Mathf.Round(vector2.y));
    }

    public static Vector3 Round(Vector3 vector3)
    {
        return new Vector3(Mathf.Round(vector3.x),Mathf.Round(vector3.y),Mathf.Round(vector3.z));
    }
}
