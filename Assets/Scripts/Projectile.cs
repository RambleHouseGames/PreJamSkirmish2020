using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float maxDistance = 10f;

    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private Transform home;

    [NonSerialized]
    public ProjectileState currentState = ProjectileState.WAITING;

    [NonSerialized]
    public Vector3 ballisticEndPoint;

    private Collider myCollider;

    void Awake()
    {
        myCollider = GetComponent<Collider>();
    }

    public void Fire()
    {
        if (currentState == ProjectileState.WAITING)
        {
            myCollider.isTrigger = true;
            ballisticEndPoint = transform.position + transform.forward + transform.forward.normalized * maxDistance;
            transform.SetParent(null);
            currentState = ProjectileState.FIRING;
        }
    }

    void Update()
    {
        if (currentState == ProjectileState.FIRING)
        {
            float distanceToEndPoint = Vector3.Distance(transform.position, ballisticEndPoint);
            if (speed * Time.deltaTime > distanceToEndPoint)
            {
                myCollider.isTrigger = false;
                transform.position = ballisticEndPoint;
                currentState = ProjectileState.RETURNING;
            }
            else
                transform.position = Vector3.MoveTowards(transform.position, ballisticEndPoint, speed * Time.deltaTime);
        }
        if (currentState == ProjectileState.RETURNING)
        {
            float distanceToHome = Vector3.Distance(transform.position, home.position);
            if(speed * Time.deltaTime > distanceToHome)
            {
                myCollider.isTrigger = false;
                transform.position = home.position;
                transform.rotation = home.rotation;
                transform.SetParent(home.parent);
                currentState = ProjectileState.WAITING;
            }
            transform.position = Vector3.MoveTowards(transform.position, home.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pirate")
        {
            other.gameObject.GetComponent<Pirate>().Donk();
            currentState = ProjectileState.RETURNING;
        }
    }
}

public enum ProjectileState { WAITING, FIRING, RETURNING }
