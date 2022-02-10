using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHaunting : NpcState1
{
    public StateHaunting(StateMachine sm, Npc1 n) : base(sm, n) { }

    public override void Awake()
    {
        base.Awake();
    }

    public override void Execute()
    {
        npc.transform.position += (npc.Target() - npc.transform.position).normalized * (npc.speed * Time.deltaTime);
        if (npc.RangeAttack())
        {
            _sm.SetState<StateAttack>();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
