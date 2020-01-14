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

    private List<Pirate> pickupList = new List<Pirate>();

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        SignalManager.Inst.AddListenner(Signal.PIRATE_DONKED, onPirateDonked);
        SignalManager.Inst.AddListenner(Signal.PIRATE_TURNED_IN, onPirateTurnedIn);
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

    private void onPirateTurnedIn(System.Object args)
    {
        Pirate turnedPirate = ((PirateTurnedInArgs)args).TurnedInPirate;
        if (currentState == MinionState.DROPOFF && turnedPirate == pickupList[0])
        {
            pickupList.Remove(turnedPirate);
            if (pickupList.Count == 0)
                currentState = MinionState.FOLLOW;
            else
                currentState = MinionState.PICKUP;
        }
        else if(pickupList.Contains(turnedPirate))
        {
            pickupList.Remove(turnedPirate);
        }
        
    }

    private void onPirateDonked(System.Object args)
    {
        PirateDonkedArgs pirateDonkedArgs = (PirateDonkedArgs)args;
        if(!pickupList.Contains(pirateDonkedArgs.DonkedPirate)) 
            pickupList.Add(pirateDonkedArgs.DonkedPirate);
        if(currentState == MinionState.FOLLOW)
        {
            currentState = MinionState.PICKUP;
        }
    }
}

public enum MinionState { FOLLOW, PICKUP, DROPOFF, DONK }
