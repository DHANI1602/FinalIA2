
using UnityEngine;
using System;
[Serializable]
public class SerializableVector4
{
    public float x, y, z, w;

    public SerializableVector4(Quaternion quaternion)
    {
        x = quaternion.x;
        y = quaternion.y;
        z = quaternion.z;
        w = quaternion.w;
    }
}
