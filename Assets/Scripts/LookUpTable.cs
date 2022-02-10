using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LookUpTable<TKey, TValue> 
{
    private Dictionary<TKey, TValue> _values = new Dictionary<TKey, TValue>();

    private Func<TKey, TValue> _process;

    public LookUpTable(Func<TKey, TValue> process)
    {
        _process = process;
    }


    public TValue Get(TKey key)
    {
        if(!_values.ContainsKey(key))
        {
            _values[key] = _process(key);
        }

        return _values[key];
    }

}
