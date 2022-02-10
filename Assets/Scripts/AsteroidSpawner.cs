using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{

    public float timer;
    public float timertospawn;
    public GameObject[] _spawnpoints;

    SpatialGrid grid;

    public ModifiedObjectPooler<Asteroid> _largeAsteroidPool;
    public ModifiedObjectPooler<Asteroid> _mediumAsteroidPool;
    public ModifiedObjectPooler<Asteroid> _smallAsteroidPool;
    private AsteroidFactory _largeAsteroidFactory = new AsteroidFactory("LargeAsteroid");
    private AsteroidFactory _mediumAsteroidFactory = new AsteroidFactory("MediumAsteroid");
    private AsteroidFactory _smallAsteroidFactory = new AsteroidFactory("SmallAsteroid");


    private void Start()
    {
        grid = FindObjectOfType<SpatialGrid>();
        _largeAsteroidPool = new ModifiedObjectPooler<Asteroid>(_largeAsteroidFactory.Create, Asteroid.TurnOn, Asteroid.TurnOff, 2);

        _smallAsteroidPool = new ModifiedObjectPooler<Asteroid>(_smallAsteroidFactory.Create, Asteroid.TurnOn, Asteroid.TurnOff, 2);

        _mediumAsteroidPool = new ModifiedObjectPooler<Asteroid>(_mediumAsteroidFactory.Create, Asteroid.TurnOn, Asteroid.TurnOff, 5);
        for (int i = 0; i < _spawnpoints.Length; i++)
        {
            var asteroid = _largeAsteroidPool.Get();
            asteroid.pool = _largeAsteroidPool;
            asteroid.transform.position = _spawnpoints[i].transform.position;
            asteroid.transform.SetParent(grid.transform);
        }

    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= timertospawn)
        {
            var rr = Random.Range(0, 4);
            var asteroid = _largeAsteroidPool.Get();
            asteroid.pool = _largeAsteroidPool;

            if(rr == 0)
            {
                asteroid.transform.position = _spawnpoints[0].transform.position;
                asteroid.transform.SetParent(grid.transform);

            }
            else if(rr == 1)
            {
                asteroid.transform.position = _spawnpoints[1].transform.position;
                asteroid.transform.SetParent(grid.transform);

            }
            else if(rr == 2)
            {
                asteroid.transform.position = _spawnpoints[2].transform.position;
                asteroid.transform.SetParent(grid.transform);

            }
            else
            {
                asteroid.transform.position = _spawnpoints[3].transform.position;
                asteroid.transform.SetParent(grid.transform);

            }
           
            timer = 0;
        }
    }
}
