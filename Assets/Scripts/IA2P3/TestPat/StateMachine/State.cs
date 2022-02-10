using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State 
{
    protected StateMachine _sm;
    public State(StateMachine sm)
    {
        _sm = sm;
    }

    public virtual void Awake() { }

    public virtual void Exit() { }

    public virtual void Execute() { }

    public virtual void LateExecute() { }

}
