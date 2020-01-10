using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlowManager : MonoBehaviour
{
    public static SceneFlowManager Inst;

    public SceneState currentState = new StartState();

    private void Awake()
    {
        if (SceneFlowManager.Inst == null)
            Inst = this;
        else
            Destroy(gameObject);
    }

    protected virtual void Start()
    {
    }

    private void Update()
    {
        if (!currentState.isStarted)
        {
            currentState.Start();
        }
    }
}

public class SceneState
{
    public bool isStarted = false;

    public void Start()
    {
        onStart();
        isStarted = true;
    }

    protected virtual void onStart()
    {
        Debug.Log("SceneState.Start not implemented");
    }
}

public class StartState : SceneState
{
    protected override void onStart()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
        SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
    }

    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "GameScene")
            SignalManager.Inst.FireSignal(Signal.GameSceneLoaded);
        SceneManager.sceneLoaded -= onSceneLoaded;
    }
}
