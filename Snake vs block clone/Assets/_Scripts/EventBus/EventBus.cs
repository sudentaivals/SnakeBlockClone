using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBus
{
    private static readonly IDictionary<EventBusEvent, UnityEvent<UnityEngine.Object, EventArgs>> Events = new Dictionary<EventBusEvent, UnityEvent<UnityEngine.Object, EventArgs>>();

    public static void Subscribe(EventBusEvent eventType, UnityAction<UnityEngine.Object, EventArgs> listener)
    {
        UnityEvent<UnityEngine.Object, EventArgs> thisEvent;
        if(Events.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<UnityEngine.Object, EventArgs>();
            thisEvent.AddListener(listener);
            Events.Add(eventType, thisEvent);
        }
    }

    public static void Unsubscribe(EventBusEvent eventType, UnityAction<UnityEngine.Object, EventArgs> listener)
    {
        UnityEvent<UnityEngine.Object, EventArgs> thisEvent;
        if(Events.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void Publish(EventBusEvent eventType, UnityEngine.Object sender, EventArgs args)
    {
        UnityEvent<UnityEngine.Object, EventArgs> thisEvent;
        if(Events.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.Invoke(sender, args);
        }
    }
}
