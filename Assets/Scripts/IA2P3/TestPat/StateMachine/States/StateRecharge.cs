using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRecharge : NpcState1
{
    public StateRecharge(StateMachine sm, Npc1 _npc1) : base(sm, _npc1) { }

    bool recharging;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Execute()
    {
        if (recharging == false)
        {

            npc.StartCoroutine(Recharging());
        }
    }

    IEnumerator Recharging()
    {
        recharging = true;
        yield return new WaitForSeconds(3f);

        npc.bulletAmmount = 3;
        recharging = false;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
