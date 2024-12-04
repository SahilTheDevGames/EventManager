using System;
using System.Collections.Generic;

public static class EventManager
{
    private static Dictionary<string, Delegate> _eventDictionary = new();

    public static void Register(string eventType, Delegate eventHandler)
    {
        if (!_eventDictionary.ContainsKey(eventType))
        {
            _eventDictionary[eventType] = eventHandler;
        }
        else
        {
            _eventDictionary[eventType] = Delegate.Combine(_eventDictionary[eventType], eventHandler);
        }
    }

    public static void Unregister(string eventType, Delegate eventHandler)
    {
        if (_eventDictionary.ContainsKey(eventType))
        {
            var currentDelegate = _eventDictionary[eventType];
            currentDelegate = Delegate.Remove(currentDelegate, eventHandler);

            if (currentDelegate == null)
            {
                _eventDictionary.Remove(eventType);
            }
            else
            {
                _eventDictionary[eventType] = currentDelegate;
            }
        }
    }

    public static void Execute(string eventType, params object[] args)
    {
        if (_eventDictionary.ContainsKey(eventType))
        {
            _eventDictionary[eventType].DynamicInvoke(args);
        }
    }
}
