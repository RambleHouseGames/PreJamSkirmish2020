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
    }

    private void Start()
    {
        currentState.Start();
    }

    private void Update()
    {
        
    }
}

public class SceneState
{
    public virtual void Start()
    {
        Debug.Log("SceneState.Start not implemented");
    }
}

public class StartState : SceneState
{
    public override void Start()
    {
        SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);
    }
}
