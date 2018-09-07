using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMessage { }

public abstract class Singleton<T> where T : class, new()
{
    private static T _instance;
    public static T Instance => _instance ?? (_instance = new T());
}

public class Messenger : Singleton<Messenger>
{
    private Dictionary<Type, List<Action<IMessage>>> _messageMappings;

    public Messenger()
    {
        _messageMappings = new Dictionary<Type, List<Action<IMessage>>>();
    }

    public void SendMessageOfType<T>(T message) where T : IMessage
    {
        var messageType = typeof(T);
        if (!_messageMappings.ContainsKey(messageType)) return;

        foreach (var listenerAction in _messageMappings[messageType])
        {
            listenerAction(message);
        }
    }

    public void SubscribeToMessageOfType<T>(Action<IMessage> messageHandler) where T : IMessage
    {
        var messageType = typeof(T);

        if (!_messageMappings.ContainsKey(messageType))
            _messageMappings.Add(messageType, new List<Action<IMessage>>());

        _messageMappings[messageType].Add(messageHandler);
    }

    public void UnsubscribeAllMessageForObject(object targetObject)
    {
        var actionLists = _messageMappings.Values;
        foreach (var actionList in actionLists)
            actionList.RemoveAll(x => x.Target == targetObject);
    }
}
