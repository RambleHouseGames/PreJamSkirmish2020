using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortcutSceneFlowManager : SceneFlowManager
{
    protected override void Start()
    {
        currentState = new ShortcutStartState();
        base.Start();
    }
}

public class ShortcutStartState : SceneState
{
    protected override void onStart()
    {
        SignalManager.Inst.FireSignal(Signal.GameSceneLoaded);
    }
}
