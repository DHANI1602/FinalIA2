using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    State _currState;

    List<State> _states = new List<State>();

    public void Update()
    {
        if (_currState != null)
            _currState.Execute();
    }

    public void LateUpdate()
    {
        if (_currState != null)
            _currState.LateExecute();
    }

    public void AddState(State a)
    {
        _states.Add(a);
        if (_currState == null)
            _currState = a;
    }
    public void SetState<T>() where T : State
    {
        for (int i = 0; i < _states.Count; i++)
        {
            if (_states[i].GetType() == typeof(T))
            {
                _currState.Exit();
                _currState = _states[i];
                _currState.Awake();
            }
        }
    }
    public bool IsActualState<T>()where T : State
    {
        return _currState.GetType() == typeof(T);
    }

    
}
