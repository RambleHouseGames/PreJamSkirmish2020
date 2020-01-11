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

        SceneState nextState = currentState.GetNextState();
        if (nextState != currentState)
        {
            currentState.Finish();
            currentState = nextState;
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

    public virtual SceneState GetNextState()
    {
        Debug.Log("SceneState.GetNextState no implemented");
        return this;
    }

    public virtual void Finish()
    {
    }
}

public class StartState : SceneState
{
    private bool menuLoaded = false;

    public override SceneState GetNextState()
    {
        if (menuLoaded)
            return new MenuState();
        else
            return this;
    }

    protected override void onStart()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Additive);
    }

    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MenuScene")
        {
            SignalManager.Inst.FireSignal(Signal.MENU_SCENE_LOADED, null);
            SceneManager.sceneLoaded -= onSceneLoaded;
            menuLoaded = true;
        }
    }
}

public class MenuState : SceneState
{
    private bool startButtonPressed = false;

    protected override void onStart()
    {
        SignalManager.Inst.AddListenner(Signal.START_BUTTON_PRESSED, onStartButtonPressed);
    }

    private void onStartButtonPressed(System.Object args)
    {
        SignalManager.Inst.RemoveListenner(Signal.START_BUTTON_PRESSED, onStartButtonPressed);
        startButtonPressed = true;
    }

    public override SceneState GetNextState()
    {
        if (startButtonPressed)
            return new GameState();
        else
            return this;
    }

    public override void Finish()
    {
        SceneManager.UnloadSceneAsync("MenuScene");
    }
}

public class GameState : SceneState
{
    protected override void onStart()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
        SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
    }

    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "GameScene")
        {
            SignalManager.Inst.FireSignal(Signal.GAME_SCENE_LOADED, null);
        }
    }

    public override SceneState GetNextState()
    {
        return this;
    }
}
