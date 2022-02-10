using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack : NpcState1
{
    public StateAttack(StateMachine sm, Npc1 i) : base(sm, i)
    {
    }
    float shootTimeInside;
    public override void Awake()
    {
        shootTimeInside = npc.timeShoot;
    }

    public override void Execute()
    {
        if (!npc.RangeAttack())
        {
            _sm.SetState<StateHaunting>();
        }
        else
        {
            TimeDelay();
        }
    }
    void TimeDelay()
    {
        shootTimeInside -= Time.deltaTime;
        if (shootTimeInside <= 0)
        {
            npc.Fire();
            shootTimeInside = npc.timeShoot;
        }
    }
    public override void Exit()
    {
        base.Exit();
    }

}
