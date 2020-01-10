using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Civilian : MonoBehaviour
{
    [SerializeField]
    private List<Vector2> waypoints;

    [SerializeField]
    private float waypointMargin = .01f;

    private NavMeshAgent navMeshAgent;

    private int currentWaypoint = 0;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(waypoints != null && waypoints.Count > 0)
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
}
