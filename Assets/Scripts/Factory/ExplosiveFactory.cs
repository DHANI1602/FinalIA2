using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveFactory : IFactory<ExplosiveBullet>
{
    public GameObject prefab;


    public ExplosiveBullet Create()
    {
        var obj = Resources.Load<ExplosiveBullet>("Prefab/Explosive Bullet");
        obj.SetProjectileSpeed(300);
        obj.SetForce(20);
        obj.SetPushExplosion(1.5f);
        obj.SetRadiusExplosion(2);
        return Object.Instantiate(obj);



    }
    
}
