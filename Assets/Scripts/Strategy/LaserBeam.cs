using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour, IThrow
{
    public GameObject laser;

    public void Throw()
    {
            laser.SetActive(true);

    }
}
