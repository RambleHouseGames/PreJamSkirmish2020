using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pirate : MonoBehaviour
{
    public PirateType myType;

    public GameObject Destination;

    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private Vector3 smoothDeltaPosition;
    private Vector3 velocity;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        navMeshAgent.updateRotation = false;
        navMeshAgent.updatePosition = false;
        navMeshAgent.destination = Destination.transform.position;
    }

    void Update()
    {

        if (haveArrivedAt(Destination.transform.position))
        {
            Destroy(gameObject);
        }

        if (navMeshAgent.isOnNavMesh)
        { 
            Vector3 worldDeltaPosition = navMeshAgent.nextPosition - transform.position;

            // Map 'worldDeltaPosition' to local space
            float dx = Vector3.Dot(transform.right, worldDeltaPosition);
            float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
            Vector2 deltaPosition = new Vector2(dx, dy);

            // Low-pass filter the deltaMove
            float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

            // Update velocity if time advances
            if (Time.deltaTime > 1e-5f)
                velocity = smoothDeltaPosition / Time.deltaTime;

            bool shouldMove = velocity.magnitude > 0.5f && navMeshAgent.remainingDistance > navMeshAgent.radius;
            animator.SetBool("AmMoving", shouldMove);
        }
    }

    public void Donk()
    {
        navMeshAgent.destination = transform.position;
        animator.SetTrigger("Donk");
        SignalManager.Inst.FireSignal(Signal.PIRATE_DONKED, new PirateDonkedArgs(this));
    }

    private bool haveArrivedAt(Vector3 destination)
    {
        return (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(destination.x, destination.z)) <= navMeshAgent.speed * Time.deltaTime);
    }

    public void Pickup(GameObject target)
    {
        navMeshAgent.enabled = false;
        transform.position = target.transform.position;
        transform.SetParent(target.transform);
    }

    public void DropOff()
    {
        SignalManager.Inst.FireSignal(Signal.PIRATE_TURNED_IN, new PirateTurnedInArgs(this));
        Destroy(gameObject);
    }

    void OnAnimatorMove()
    {
        transform.position = navMeshAgent.nextPosition;
    }
}