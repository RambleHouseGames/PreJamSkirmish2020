using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SignalManager : MonoBehaviour
{
    public static SignalManager Inst;

    private Dictionary<Signal, Action<System.Object>> listenners = new Dictionary<Signal, Action<System.Object>>();

    private void Awake()
    {
        if (SignalManager.Inst == null)
            Inst = this;
        else
            Destroy(gameObject);
    }

    public void AddListenner(Signal signal, Action<System.Object> callback)
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

    public void RemoveListenner(Signal signal, Action<System.Object> callback)
    {
        listenners[signal] -= callback;
        if (listenners[signal] == null)
            listenners.Remove(signal);
    }

    public void FireSignal(Signal signal, System.Object args)
    {
        if(listenners.ContainsKey(signal))
            listenners[signal](args);
    }
}

public enum Signal { STARTED_LOADING, GAME_SCENE_LOADED, MENU_SCENE_LOADED, CIVILIAN_DONKED, PIRATE_DONKED, CIVILIAN_TURNED_IN, PIRATE_TURNED_IN, START_BUTTON_PRESSED, GAME_OVER}

public class CivilianDonkedArgs
{
    public Civilian DonkedCivilian;

    public CivilianDonkedArgs(Civilian donkedCivilian)
    {
        DonkedCivilian = donkedCivilian;
    }
}

public class CivilianTurnedInArgs
{
    public Civilian TurnedInCivilian;

    public CivilianTurnedInArgs(Civilian turnedInCivilian)
    {
        TurnedInCivilian = turnedInCivilian;
    }
}

public class PirateDonkedArgs
{
    public Pirate DonkedPirate;

    public PirateDonkedArgs(Pirate donkedPirate)
    {
        DonkedPirate = donkedPirate;
    }
}

public class PirateTurnedInArgs
{
    public Pirate TurnedInPirate;

    public PirateTurnedInArgs(Pirate turnedInPirate)
    {
        TurnedInPirate = turnedInPirate;
    }
}

public class GameOverArgs
{
    public int BountiesCollected;

    public GameOverArgs(int bountiesCollected)
    {
        BountiesCollected = bountiesCollected;
    }
}