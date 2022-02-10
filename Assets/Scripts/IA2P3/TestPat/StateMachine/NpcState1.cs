using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcState1 : State
{
    protected Npc1 npc;

    public NpcState1(StateMachine sm, Npc1 i) : base(sm)
    {
        npc = i;
    }
}
