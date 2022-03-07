using System.Collections.Generic;
using FSM;
using UnityEngine;
using System;

public class GOAPAction
{
    public Dictionary<string, Func<GOAPVariable, bool>> preconditions { get; private set; }
    public Dictionary<string, Action<GOAPVariable>> effects { get; private set; }
    public string name { get; private set; }
    public float cost { get; private set; }
    public IState linkedState { get; private set; }


    public GOAPAction(string name)
    {
        this.name = name;
        cost = 1f;
        preconditions = new Dictionary<string, Func<GOAPVariable, bool>>();
        effects = new Dictionary<string, Action<GOAPVariable>>();
    }

    public GOAPAction Cost(float cost)
    {
        if (cost < 1f)
        {

            Debug.Log(string.Format("Warning: Using cost < 1f for '{0}' could yield sub-optimal results", name));
        }

        this.cost = cost;
        return this;
    }

    public GOAPAction Pre(string s, Func<GOAPVariable, bool> value)
    {
        preconditions[s] = value;
        return this;
    }

    public GOAPAction Effect(string s, Action<GOAPVariable> value)
    {
        effects[s] = value;
        return this;
    }

    public GOAPAction LinkedState(IState state)
    {
        linkedState = state;
        return this;
    }
}
