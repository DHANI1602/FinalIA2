using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRunAway : NpcState1
{
    public StateRunAway(StateMachine sm, Npc1 i) : base(sm, i)
    {
    }

    public override void Awake()
    {
        base.Awake();
    }

    public override void Execute()
    {
        if (npc.Charging())
        {
            _sm.SetState<StateRecharge>();
        }
        else
        {
            BackOff();
        }
    }
    void BackOff()
    {
        npc.transform.position -= (npc.Target() - npc.transform.position).normalized * (npc.speed * Time.deltaTime);

    }
    public override void Exit()
    {
        base.Exit();
    }
}
