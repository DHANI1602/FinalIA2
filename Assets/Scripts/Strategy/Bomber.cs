using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour, IThrow
{
    public PlayerController _playerController;


    public void Throw()
    {
        var bullet = _playerController._explosiveBulletPool.Get();
        bullet._explosivepool = _playerController._explosiveBulletPool;

        bullet.transform.position = _playerController._playerModel.fireposition.transform.position;
        bullet.transform.rotation = transform.rotation;
    }

}
