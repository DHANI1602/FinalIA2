using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMelee : NpcState1
{
    public StateMelee(StateMachine sm, Npc1 i) : base(sm, i)
    {
    }
    float powerSpeed;
    float time = 0;
    public override void Awake()
    {
        base.Awake();
    }

    public override void Execute()
    {
        if (npc.Charging() && npc.RangeAttack())
        {
            Attack();
        }
    }
    void Attack()
    {
        time += Time.deltaTime * 2;
        powerSpeed = Mathf.Lerp(npc.speed, 4, time);
        Debug.Log(powerSpeed);
        npc.transform.position += (npc.Target() - npc.transform.position).normalized * (powerSpeed * Time.deltaTime);
    }

}
