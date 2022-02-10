using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcState2 : State
{
    protected Npc2 npc2;
    public NpcState2(StateMachine sm, Npc2 _npc2) : base(sm)
    {
        npc2 = _npc2;
    }
}
