using UnityEngine;
using System.Collections.Generic;

class EventManager : SingletonCommon<EventManager>
{
    private Dictionary<EVENT_TYPE, List<IListener>> Listeners = new();


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void AddListener(EVENT_TYPE type, IListener listener)
    {
        List<IListener> listenerlist = new();

        if(Listeners.TryGetValue(type, out listenerlist))
        {
            Listeners[type].Add(listener);
            return;
        }

        listenerlist = new() { listener };
        Listeners.Add(type, listenerlist);
    }

    public void PostNotification(EVENT_TYPE eventType, object param = null)
    {
        List<IListener> ListenList = null;

        if (!Listeners.TryGetValue(eventType, out ListenList))
            return;


        for (int i = 0; i < ListenList.Count; i++)
            ListenList?[i].OnEvent(eventType,  param);
    }

    public void RemoveEvent(EVENT_TYPE eventType) => Listeners.Remove(eventType);

    public void RemoveRedundancies()
    {
        Dictionary<EVENT_TYPE, List<IListener>> newListeners = new();

        foreach (KeyValuePair<EVENT_TYPE, List<IListener>> Item in Listeners)
        {
            for (int i = Item.Value.Count - 1; i >= 0; i--)
            {
                if (Item.Value[i].Equals(null))
                    Item.Value.RemoveAt(i);
            }

            if (Item.Value.Count > 0)
                newListeners.Add(Item.Key, Item.Value);
        }

        Listeners = newListeners;
    }

    public void OnLevelWasLoaded()
    {
        RemoveRedundancies();
    }
}