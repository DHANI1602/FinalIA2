using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POwerUpSpawner : MonoBehaviour
{
    public float timer;
    public float timertospawn;
    public GameObject[] _spawnpoints;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timertospawn)
        {


            var rr = Random.Range(0, 2);
            var rp = Random.Range(0, 2);

            var obj = Resources.Load<LifePack>("Prefab/pill_red");
            var obj1 = Resources.Load<GoBack>("Prefab/pill_blue");

            if (rr == 0)
            {
                if(rp == 1)
                {

                    Instantiate(obj, _spawnpoints[0].transform.position, _spawnpoints[0].transform.rotation);
                }
                else
                {
                    Instantiate(obj1, _spawnpoints[0].transform.position, _spawnpoints[0].transform.rotation);
                }
            }
            else
            {
                if (rp == 1)
                {
                    Instantiate(obj, _spawnpoints[0].transform.position, _spawnpoints[0].transform.rotation);
                }
                else
                {
                    Instantiate(obj1, _spawnpoints[0].transform.position, _spawnpoints[0].transform.rotation);
                }
            }
          
            timer = 0;
        }
    }
}
