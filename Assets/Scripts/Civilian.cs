using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System;

public class Civilian : MonoBehaviour
{
    [SerializeField]
    private List<Vector2> waypoints;

    [SerializeField]
    private float waypointMargin = .01f;

    [SerializeField]
    private List<CivilianType> civilianTypeOptions;

    [SerializeField]
    private bool shouldRandomizeWaypoints = true;

    [SerializeField]
    private bool shouldRandomizeType = true;

    [SerializeField]
    public CivilianType civilianType;

    [SerializeField]
    private Material InnocentCivilianMaterial;

    [SerializeField]
    private Material OrangeCivilianMaterial;

    [SerializeField]
    private Material BlackCivilianMaterial;

    [SerializeField]
    private Material BlueCivilianMaterial;

    [SerializeField]
    private Material DonkedCivilianMaterial;

    [NonSerialized]
    public CivilianState currentState = CivilianState.PATROL;

    private NavMeshAgent navMeshAgent;
    private Renderer myRenderer;

    private int currentWaypoint = 0;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if(shouldRandomizeType)
        {
            int rand = UnityEngine.Random.Range(0, civilianTypeOptions.Count);
            civilianType = civilianTypeOptions[rand];
        }

        if(shouldRandomizeWaypoints)
        {
            for (int i = 0; i < waypoints.Count; i++)
            {
                waypoints[i] = new Vector2(UnityEngine.Random.Range(-30, 30), UnityEngine.Random.Range(-30, 30));
            }
        }

        myRenderer = GetComponent<Renderer>();

        switch (civilianType)
        {
            case CivilianType.INNOCENT:
                myRenderer.material = InnocentCivilianMaterial;
                break;
            case CivilianType.ORANGE:
                myRenderer.material = OrangeCivilianMaterial;
                break;
            case CivilianType.BLACK:
                myRenderer.material = BlackCivilianMaterial;
                break;
            case CivilianType.BLUE:
                myRenderer.material = BlueCivilianMaterial;
                break;
        }
    }

    void Update()
    {
        if (currentState == CivilianState.PATROL)
        {
            patrolWaypoints();
        }
        if (currentState == CivilianState.DONKED)
        {
            //Do Donked stuff
        }
        if(currentState == CivilianState.CARRY)
        {
            //Do Carry stuff
        }
        if(currentState == CivilianState.DROPOFF)
        {
            goToJail();
        }
    }

    public void Donk()
    {
        navMeshAgent.destination = transform.position;
        myRenderer.material = DonkedCivilianMaterial;
        currentState = CivilianState.DONKED;
        SignalManager.Inst.FireSignal(Signal.CIVILIAN_DONKED, new CivilianDonkedArgs(this));
    }

    public void Pickup(GameObject target)
    {
        currentState = CivilianState.CARRY;
        navMeshAgent.enabled = false;
        transform.position = target.transform.position;
        transform.SetParent(target.transform);
    }

    public void DropOff()
    {
        currentState = CivilianState.DROPOFF;
        navMeshAgent.enabled = true;
        navMeshAgent.destination = Jail.Inst.transform.position;
    }

    private void patrolWaypoints()
    {
        if (waypoints != null && waypoints.Count > 0)
        {
            Vector2 destination = waypoints[currentWaypoint];
            navMeshAgent.destination = new Vector3(destination.x, 0f, destination.y);
            float distanceToWaypoint = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), waypoints[currentWaypoint]);
            if (distanceToWaypoint <= waypointMargin)
            {
                currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
            }
        }
    }

    private void goToJail()
    {
        if(Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(Jail.Inst.transform.position.x, Jail.Inst.transform.position.z)) <= navMeshAgent.speed * Time.deltaTime)
        {
            SignalManager.Inst.FireSignal(Signal.CIVILIAN_TURNED_IN, new CivilianTurnedInArgs(this));
            Destroy(gameObject);
        }
    }
}

public enum CivilianState { PATROL, DONKED, CARRY, DROPOFF }
public enum CivilianType { INNOCENT, ORANGE, BLACK, BLUE }
