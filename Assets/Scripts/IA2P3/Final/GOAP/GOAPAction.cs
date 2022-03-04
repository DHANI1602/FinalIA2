﻿using System.Collections.Generic;
using FSM;
using UnityEngine;
using System;

public class GOAPAction {


    public delegate bool CheckPrecondition(object obj);

    public delegate void DoEffect(bool obj);

    public Dictionary<string, CheckPrecondition> preconditions { get; private set; }
    public Dictionary<string, object> effects       { get; private set; }
    public string                   name          { get; private set; }
    public float                    cost          { get; private set; }
    public IState                   linkedState   { get; private set; }

    public Func<GOAPState, bool> preconditionLamnda { get; private set; }


    public GOAPAction(string name) {
        this.name     = name;
        cost          = 1f;
        preconditions = new Dictionary<string, CheckPrecondition>();
        effects       = new Dictionary<string, object>();
    }

    public GOAPAction Cost(float cost) {
        if (cost < 1f) {
            //Costs < 1f make the heuristic non-admissible. h() could overestimate and create sub-optimal results.
            //https://en.wikipedia.org/wiki/A*_search_algorithm#Properties
            Debug.Log(string.Format("Warning: Using cost < 1f for '{0}' could yield sub-optimal results", name));
        }

        this.cost = cost;
        return this;
    }

    public GOAPAction Pre(string s, CheckPrecondition value) {
        preconditions[s] = value;
        return this;
    }

    public GOAPAction Effect(string s, bool value) {
        effects[s] = value;
        return this;
    }

    public GOAPAction LinkedState(IState state) {
        linkedState = state;
        return this;
    }
}
