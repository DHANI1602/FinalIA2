using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rb2d;
    public Vector3 dir;
    public float projectilespeed;
    public ModifiedObjectPooler<Bullet> pool;
    public ModifiedObjectPooler<ExplosiveBullet> _explosivepool;
    public float _spawnTime;
    public float timeToDestroy;
    public PlayerController player;
    BulletFlyWeight flyweight;


    public Bullet SetProjectileSpeed(float newspeed)
    {
        projectilespeed = newspeed;
        return this;
    }
    public Bullet SetTimeToDestroy(float newTimeToDestroy)
    {
        timeToDestroy = newTimeToDestroy;
        
        return this;
    }
    public void Save()
    {
        var data = new BulletData();
        data.position = new SerializableVector3(transform.position);
        data.direction = new SerializableVector3(dir);
        data.spawntime = _spawnTime;


        BinarySerializer.SaveBinary(data, $"{Application.dataPath}\\Resources\\BulletData");
    }
    public void Load()
    {
        var data = BinarySerializer.LoadBinary<BulletData>($"{Application.dataPath}\\Resources\\BulletData");

        transform.position = new Vector3(data.position.x, data.position.y, data.position.z);
        dir = new Vector3(data.position.x, data.position.y, data.position.z);
        _spawnTime = data.spawntime;

    }



    protected virtual void OnEnable()
    {
        _spawnTime = Time.time;
        player = FindObjectOfType<PlayerController>();
        dir = player.dir;
        _rb2d = GetComponent<Rigidbody2D>();
        _rb2d.AddForce(dir * BulletFlyweightPointer.bulletFlyweight.projectilespeed);
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
        if(gameObject.activeInHierarchy)
       pool.ReturnToPool(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<GoapCircularEnemy>() != null)
        {
            collision.gameObject.GetComponent<GoapCircularEnemy>().TakeDamage();
        }
        gameObject.SetActive(false);
    }
    public static void TurnOn(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }
    public static void TurnOff(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }



}





