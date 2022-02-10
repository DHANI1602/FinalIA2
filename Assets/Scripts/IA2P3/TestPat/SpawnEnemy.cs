using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public float timeSpawn;

    public List<GameObject> enemies = new List<GameObject>();

    public List<GameObject> spawnPoints = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(Instantiator());
    }


    void InstantiateEnemy()
    {

        var enemy = Random.Range(0, enemies.Count);
        var point = Random.Range(0, spawnPoints.Count);

        Instantiate(enemies[enemy], spawnPoints[point].transform.position, Quaternion.identity);
    }

    IEnumerator Instantiator()
    {
        yield return new WaitForSeconds(timeSpawn);
        InstantiateEnemy();
        StartCoroutine(Instantiator());
    }
}
