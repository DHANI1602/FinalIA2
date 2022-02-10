using System;
using FSM;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackState : MonoBaseState, IState
{
    public override event Action OnNeedsReplan;


    float shootTimeInside = 3;
    float timeShoot = 3;
    ModifiedObjectPooler<BulletEnemy> _bulletPool;
    public float maxDistance = 3;
    PlayerController _player;

    GoapCircularEnemy gop;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        gop = GetComponent<GoapCircularEnemy>();
        var factory = new BulletEnemyFactory();
        _bulletPool = new ModifiedObjectPooler<BulletEnemy>(factory.Create, BulletEnemy.TurnOn, BulletEnemy.TurnOff, 12);
    }
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        gop.sprite.color = Color.red;
    }
    public override void UpdateLoop()
    {
        shootTimeInside -= Time.deltaTime;
        if (shootTimeInside <= 0)
        {
            var bullet = _bulletPool.Get();
            bullet.pool = _bulletPool;
            bullet.transform.position = transform.position;
             bullet.transform.rotation = transform.rotation;
            shootTimeInside = timeShoot;
            gop.ammo -= 1;
        }
        var sqrDistance = (_player.transform.position - transform.position).sqrMagnitude;

        if (sqrDistance > maxDistance * maxDistance)
        {
            gop.sprite.color = Color.white;
            OnNeedsReplan?.Invoke();
            
            
        }

    }

    public override IState ProcessInput()
    {
        if (gop.life == 1 && Transitions.ContainsKey("RunAway"))
        {
           // gop.sprite.color = Color.green;
            return Transitions["RunAway"];
        }

        else if(gop.ammo <= 0 && Transitions.ContainsKey("Recharge"))
        {
           // gop.sprite.color = Color.white;
            return Transitions["Recharge"];
        }

        return this;
    }
}