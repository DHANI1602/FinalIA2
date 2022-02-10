using System;
using FSM;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RechargeWeaponState : MonoBaseState, IState
{
    GoapCircularEnemy gop;
    public float rechargeTimer;
    float currentRechargeTimer;
    private PlayerController _player;

    private void Awake()
    {
        gop = GetComponent<GoapCircularEnemy>();
        _player = FindObjectOfType<PlayerController>();
    }

    public override void UpdateLoop()
    {
        if (currentRechargeTimer < rechargeTimer)
        {
            currentRechargeTimer += Time.deltaTime;
        }
        else
        {
            if (gop.ammo < 3)
            {

                gop.ammo += 1;
                currentRechargeTimer = 0;
            }
        }
    }

    public override IState ProcessInput()
    {
        var sqrDistance = (_player.transform.position - transform.position).sqrMagnitude;

        if (gop.ammo == 3 && sqrDistance < gop.chaseState.rangeDistance && Transitions.ContainsKey("RangeAttackState"))
        {
            return Transitions["RangeAttackState"];
        }
        else if( sqrDistance > gop.chaseState.rangeDistance && sqrDistance > 10 && Transitions.ContainsKey("PatrolState"))
        {
            return Transitions["PatrolState"];
        }
        else if(sqrDistance < gop.chaseState.rangeDistance && Transitions.ContainsKey("ChaseState"))
        {
            return Transitions["ChaseState"];
        }
        return this;
    }
}