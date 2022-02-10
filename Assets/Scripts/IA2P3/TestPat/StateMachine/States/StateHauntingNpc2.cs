using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHauntingNpc2 : NpcState2
{
    public StateHauntingNpc2(StateMachine sm, Npc2 _npc2) : base(sm, _npc2) { }

    public override void Awake()
    {
        base.Awake();
    }

    public override void Execute()
    {
        if (npc2.RangeAttack())
        {
            _sm.SetState<StateKamikaze>();
        }
        else
        {
            Move();
        }
    }

    void Move()
    {
        npc2.motorfire.SetActive(true);
        npc2.transform.position += (npc2.Target() - npc2.transform.position).normalized * (npc2.speed * Time.deltaTime);
    }
}
