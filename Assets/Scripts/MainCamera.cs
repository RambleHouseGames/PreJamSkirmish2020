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

    public static MainCamera Inst;

    public Camera CameraComponent;

    private GameObject player;

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
        float marginSize = Screen.width * scrollMarginPer;

        if (Input.mousePosition.x < marginSize)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (ScrollMaxSpeed * Time.deltaTime));
        }
        if (Input.mousePosition.x > Screen.width - marginSize)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (ScrollMaxSpeed * Time.deltaTime));
        }
        if(Input.mousePosition.y < marginSize)
        {
            transform.position = new Vector3(transform.position.x + (ScrollMaxSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        if (Input.mousePosition.y > Screen.height - marginSize)
        {
            transform.position = new Vector3(transform.position.x - (ScrollMaxSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        }
    }

    private void onGameSceneLoaded(System.Object args)
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        transform.position = player.transform.position + startingOffset;
        transform.LookAt(player.transform.position);
    }
}
