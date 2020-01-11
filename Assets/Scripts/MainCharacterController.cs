using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainCharacterController : MonoBehaviour
{
    [SerializeField]
    private Projectile projectile;

    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (projectile.currentState == ProjectileState.WAITING)
        {
            navMeshAgent.isStopped = false;
            if (Input.GetMouseButtonUp(1))
            {
                Vector3? mouseGroundPoint = RayCastCameraToGround();
                if (mouseGroundPoint != null)
                {
                    navMeshAgent.destination = (Vector3)mouseGroundPoint;
                }
            }

            if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl))
            {
                navMeshAgent.updateRotation = false;
                Vector3? mouseGroundPoint = RayCastCameraToGround();
                if (mouseGroundPoint != null)
                {
                    Vector3 lookPoint = new Vector3(((Vector3)mouseGroundPoint).x, transform.position.y, ((Vector3)mouseGroundPoint).z);
                    transform.LookAt(lookPoint);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    projectile.Fire();
                }
            }
            else
                navMeshAgent.updateRotation = true;
        }
        else
        {
            navMeshAgent.destination = transform.position;
            navMeshAgent.isStopped = true;
        }
    }

    private Vector3? RayCastCameraToGround()
    {
        Ray ray = MainCamera.Inst.CameraComponent.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return null;
    }
}
