using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public delegate void Callback(params object[] parameters);

    private static Dictionary<string, Callback> _events = new Dictionary<string, Callback>();

    public static void Subscribe(string eventId, Callback callback)
    {
        if (!_events.ContainsKey(eventId))
        {
            _events.Add(eventId, callback);
        }
        else
        {
            _events[eventId] += callback;
        }
    }

    public static void Unsubscribe(string eventId, Callback callback)
    {
        if (_events.ContainsKey(eventId))
        {
            _events[eventId] -= callback;
        }
    }

    public static void Trigger(string eventId, params object[] parameters)
    {
        if (_events.ContainsKey(eventId))
        {
            _events[eventId](parameters);
        }
    }

    public static void ClearDic()
    {
        _events.Clear();
    }
}
