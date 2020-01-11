using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour
{
    [SerializeField]
    private GameObject playerFollowTarget;

    [SerializeField]
    private GameObject jailTarget;

    [SerializeField]
    private GameObject pickupTarget;

    [NonSerialized]
    public MinionState currentState = MinionState.FOLLOW;

    private NavMeshAgent navMeshAgent;

    private List<Civilian> pickupList = new List<Civilian>();

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        SignalManager.Inst.AddListenner(Signal.CIVILIAN_DONKED, onCivilianDonked);
        SignalManager.Inst.AddListenner(Signal.CIVILIAN_TURNED_IN, onCivilianTurnedIn);
    }

    void Update()
    {
        if (currentState == MinionState.FOLLOW)
            navMeshAgent.destination = playerFollowTarget.transform.position;

        if (currentState == MinionState.PICKUP)
        {
            navMeshAgent.destination = pickupList[0].transform.position;
            if (Vector3.Distance(transform.position, navMeshAgent.destination) <= 1.5f) //Tested the touching distance in the editor
            {
                pickupList[0].Pickup(pickupTarget);
                currentState = MinionState.DROPOFF;
            }
        }

        if (currentState == MinionState.DROPOFF)
        {
            navMeshAgent.destination = jailTarget.transform.position;
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(jailTarget.transform.position.x, jailTarget.transform.position.z)) <= navMeshAgent.speed * Time.deltaTime)
            {
                pickupList[0].DropOff();
            }
        }
    }

    private void onCivilianTurnedIn(System.Object args)
    {
        Civilian turnedInCivilian = ((CivilianTurnedInArgs)args).TurnedInCivilian;
        if (currentState == MinionState.DROPOFF && turnedInCivilian == pickupList[0])
        {
            pickupList.Remove(turnedInCivilian);
            if (pickupList.Count == 0)
                currentState = MinionState.FOLLOW;
            else
                currentState = MinionState.PICKUP;
        }
        else if(pickupList.Contains(turnedInCivilian))
        {
            pickupList.Remove(turnedInCivilian);
        }
        
    }

    private void onCivilianDonked(System.Object args)
    {
        CivilianDonkedArgs civilianDonkedArgs = (CivilianDonkedArgs)args;
        if(!pickupList.Contains(civilianDonkedArgs.DonkedCivilian)) 
            pickupList.Add(civilianDonkedArgs.DonkedCivilian);
        if(currentState == MinionState.FOLLOW)
        {
            currentState = MinionState.PICKUP;
        }
    }
}

public enum MinionState { FOLLOW, PICKUP, DROPOFF, DONK }
