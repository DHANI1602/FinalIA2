using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    private Rigidbody2D _rb2d;
    public Vector3 dir;
    public float projectilespeed;
    public ModifiedObjectPooler<BulletEnemy> pool;
    public float _spawnTime;
    public float timeToDestroy;
    PlayerController _player;
    BulletFlyWeight flyweight;

    bool rutine;

    public BulletEnemy SetProjectileSpeed(float newspeed)
    {
        projectilespeed = newspeed;
        return this;
    }

    public BulletEnemy SetTimeToDestroy(float newTimeToDestroy)
    {
        timeToDestroy = newTimeToDestroy;

        return this;
    }

    protected virtual void OnEnable()
    {
        
        _spawnTime = Time.time;
        StartCoroutine(Bullet());
    }
    private void Update()
    {
        if(rutine == false)
        {
            return;
        }
        transform.position += dir * 4 * Time.deltaTime;
    }

    protected virtual void LateUpdate()
    {
        if (_spawnTime + timeToDestroy <= Time.time)
        {
            Explode();
        }
    }

    protected virtual void Explode()
    {
        if (gameObject.activeInHierarchy)
            pool.ReturnToPool(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 11)


        gameObject.SetActive(false);
    }

    public static void TurnOn(BulletEnemy bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    public static void TurnOff(BulletEnemy bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    IEnumerator Bullet()
    {
        yield return new WaitForSeconds(0.1f);
        _player = FindObjectOfType<PlayerController>();
        dir = (_player.transform.position - transform.position).normalized;
        rutine = true;
    }
}
