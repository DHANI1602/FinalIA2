using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFactory
{
    private Asteroid obj;
    private LookUpTable<string, Asteroid> lookUpAsteroids;
    private string _type;



    public AsteroidFactory(string type)
    {
        _type = type;

    }

    public Asteroid Create()
    {
        if (lookUpAsteroids == null)
        {
            lookUpAsteroids = new LookUpTable<string, Asteroid>(GetPrefab);

        }
        obj = Object.Instantiate(lookUpAsteroids.Get(_type));
        Asteroid asteroidbuild;
        if (_type == "LargeAsteroid")
        {

            var rr = Random.Range(50, 80);
            asteroidbuild = obj.SetMaxThrust(rr);
            asteroidbuild = obj.SetMaxTorque(14);

        }
        else if (_type == "MediumAsteroid")
        {
            var rr = Random.Range(10, 20);
            asteroidbuild = obj.SetMaxThrust(rr);
            asteroidbuild = obj.SetMaxTorque(10);
        }
        else
        {
            var rr = Random.Range(4, 12);
            asteroidbuild = obj.SetMaxThrust(rr);
            asteroidbuild = obj.SetMaxTorque(300);
        }
        return asteroidbuild;

    }

    private Asteroid GetPrefab(string name)
    {
        var prefab = Resources.Load<Asteroid>("Prefab/" + name);
        return prefab;
    }
}
