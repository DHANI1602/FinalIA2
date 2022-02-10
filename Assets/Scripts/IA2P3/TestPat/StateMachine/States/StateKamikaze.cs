using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateKamikaze : NpcState2
{
    public StateKamikaze(StateMachine sm, Npc2 _npc2) : base(sm, _npc2) { }

    float powerSpeed;
    float time = 0;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Execute()
    {
        Attack();
    }

    void Attack()
    {
        time += Time.deltaTime * 2;
        powerSpeed = Mathf.Lerp(npc2.speed, 4, time);
        npc2.transform.position += (npc2.Target() - npc2.transform.position).normalized * (powerSpeed * Time.deltaTime);
    }
}
