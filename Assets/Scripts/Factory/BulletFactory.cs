using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : IFactory<Bullet>
{

    public GameObject prefab;

    //profe aqui teniamos velocidad de la bala seteada con builder, pero como tenemos que utilizar flyweight pusimos algunas cosas con flyweight y algunas con builder
    public Bullet Create()
    {
        var obj = Resources.Load<Bullet>("Prefab/Bullet");
        obj.SetTimeToDestroy(3f);
      //  obj.SetProjectileSpeed(300);
        return Object.Instantiate(obj);
       


    }

}
