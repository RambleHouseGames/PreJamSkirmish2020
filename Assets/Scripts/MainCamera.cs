using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField]
    private Vector3 startingOffset = new Vector3();

    [SerializeField]
    private float scrollMarginPer = .05f;

    [SerializeField]
    private float ScrollMaxSpeed = 1f;

    [SerializeField]
    private float smoothTime = .3f;

    public static MainCamera Inst;

    [System.NonSerialized]
    public Camera CameraComponent;

    private GameObject player;

    private Vector3 velocity;

    void Awake()
    {
        if (MainCamera.Inst == null)
            Inst = this;
        else
            Destroy(gameObject);

        CameraComponent = GetComponent<Camera>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        SignalManager.Inst.AddListenner(Signal.GAME_SCENE_LOADED, onGameSceneLoaded);
    }

    private void Update()
    {
        if(SceneFlowManager.Inst.currentState.GetType() == typeof(GameState))
            updateCameraPosition();
    }

    private void onGameSceneLoaded(System.Object args)
    {
        updateCameraPosition();
    }

    private void updateCameraPosition()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if(player != null)
            {
                transform.position = player.transform.position + startingOffset;
                transform.LookAt(player.transform.position);
            }
        }
        else
        {
            Vector3 targetPosition = new Vector3(player.transform.position.x + startingOffset.x, transform.position.y, player.transform.position.z + startingOffset.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            //transform.LookAt(player.transform.position);
        }
    }
}
