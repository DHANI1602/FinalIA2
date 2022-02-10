using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerData 
{
    public SerializableVector3 position;
    public SerializableVector4 rotation;
    public float timer;
    public float timer1;
    public bool isFiring;
    public bool isCrashed;
}
