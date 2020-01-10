using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SignalManager : MonoBehaviour
{
    public static SignalManager Inst;

    private Dictionary<Signal, Action> listenners = new Dictionary<Signal, Action>();

    private void Awake()
    {
        if (SignalManager.Inst == null)
            Inst = this;
        else
            Destroy(gameObject);
    }

    public void AddListenner(Signal signal, Action callback)
    {
        if (!listenners.ContainsKey(signal))
        {
            listenners.Add(signal, callback);
        }
        else
        {
            listenners[signal] += callback;
        }
    }

    public void RemoveListenner(Signal signal, Action callback)
    {
        listenners[signal] -= callback;
        if (listenners[signal] == null)
            listenners.Remove(signal);
    }

    public void FireSignal(Signal signal)
    {
        listenners[signal]();
    }
}

public enum Signal { StartedLoading, GameSceneLoaded }
