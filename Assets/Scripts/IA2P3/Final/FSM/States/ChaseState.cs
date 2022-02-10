using System.Collections.Generic;
using FSM;
using UnityEngine;

public class ChaseState : MonoBaseState, IState
{

    public float speed = 2f;

    public float rangeDistance = 3;

    public PlayerController _player;
    GoapCircularEnemy gop;



    private void Awake()
    {

        gop = GetComponent<GoapCircularEnemy>();
        _player = FindObjectOfType<PlayerController>();

    }
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        gop.sprite.color = Color.red;
    }


public override void UpdateLoop()
    {
        var dir = (_player.transform.position - transform.position).normalized;

        transform.position += dir * (speed * Time.deltaTime);
    }
    

    public override IState ProcessInput()
    {
        var sqrDistance = (_player.transform.position - transform.position).sqrMagnitude;

        if (gop.ammo < 1 && Transitions.ContainsKey("Recharge"))
        {
           // gop.sprite.color = Color.white;
            return Transitions["Recharge"];
        }
        else if (sqrDistance < rangeDistance * rangeDistance && Transitions.ContainsKey("OnRangeAttackState"))
        {

            return Transitions["OnRangeAttackState"];
        }

        else if(gop.life < 2 && Transitions.ContainsKey("RunAway"))
        {
           // gop.sprite.color = Color.green;
            return Transitions["RunAway"];
        }
       


        return this;
    }
}