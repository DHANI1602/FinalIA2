using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc1 : MonoBehaviour
{
    #region npcStats
    public int life;
    public float speed;
    public float radius;
    public float rangeAttack;
    public LayerMask player;
    public float timeShoot;
    public Transform spawnPosition;
    public int bulletAmmount = 5;
    public PlayerController playerCt;
    #endregion

    ModifiedObjectPooler<BulletEnemy> _bulletPool;
    StateMachine _sm;
    private void Awake()
    {
        playerCt = FindObjectOfType<PlayerController>();
        var factory = new BulletEnemyFactory();
        _bulletPool = new ModifiedObjectPooler<BulletEnemy>(factory.Create, BulletEnemy.TurnOn, BulletEnemy.TurnOff, 12);
        _sm = new StateMachine();
        _sm.AddState(new StateHaunting(_sm, this));
        _sm.AddState(new StateAttack(_sm, this));
        _sm.AddState(new StateRecharge(_sm, this));
        _sm.AddState(new StateMelee(_sm, this));
        _sm.AddState(new StateRunAway(_sm, this));
        _sm.SetState<StateHaunting>();
    }

    private void Update()
    {
        _sm.Update();
    }
    public Vector3 Target()
    {
        var target = Physics2D.OverlapCircle(transform.position, radius, player);
        return target.transform.position;
    }
    public bool Charging()
    {
        return bulletAmmount <= 0;
    }
    public bool Run()
    {
        return life <= 1;
    }
    public bool RangeAttack()
    {
        return Vector3.Distance(Target(), transform.position) < rangeAttack;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            life -= 1;
        }
        if (life <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            gameObject.SetActive(false);
        }
    }

    public void Fire()
    {
        var bullet = _bulletPool.Get();
        bullet.pool = _bulletPool;
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;
        bullet.transform.up = transform.up;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangeAttack);
    }
}
