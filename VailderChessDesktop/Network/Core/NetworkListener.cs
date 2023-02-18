using System;
using System.Collections.Concurrent;
using VailderChessDesktop.Network.Contract;
using VailderChessDesktop.Network.Core;

namespace VailderChessDesktop.Network.Core;

public class NetworkListener
{
    private ConcurrentDictionary<string, Action<object>> _actions;

    public delegate void ClientConnectedEvent();
    public event ClientConnectedEvent? ClientConnected;
    
    public delegate void ClientOnEvent(Message message);
    public event ClientOnEvent? ClientOn;
    
    public delegate void ClientEmitEvent(Message message);
    public event ClientEmitEvent? ClientEmit;

    public NetworkListener(ConcurrentDictionary<string, Action<object>> actions)
    {
        _actions = actions;
    }
    
    public NetworkListener()
    {
        _actions = new ConcurrentDictionary<string, Action<object>>();
    }

    // private Action<Packet> this[string message]
    // {
    //     set
    //     {
    //         if (!_actions.ContainsKey(message))
    //             _actions.TryAdd(message, value);
    //         else
    //             _actions[message] = value;
    //
    //     }
    // }
    //
    // public void On(string message, Action<object> action)
    // {
    //     this[message] = action;
    // }

    public virtual void OnClientConnected()
    {
        ClientConnected?.Invoke();
    }
    
    public virtual void OnClientOn(Message message)
    {
        ClientOn?.Invoke(message);
    }

    public virtual void OnClientEmit(Message message)
    {
        ClientEmit?.Invoke(message);
    }
}