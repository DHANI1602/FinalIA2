using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemyFactory : IFactory<BulletEnemy>
{
    public GameObject prefab;
    public BulletEnemy Create()
    {
        var obj = Resources.Load<BulletEnemy>("Prefab/BulletEnemy");
        obj.SetTimeToDestroy(3f);
        return Object.Instantiate(obj);
    }
}
