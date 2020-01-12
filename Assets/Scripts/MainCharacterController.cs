using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainCharacterController : MonoBehaviour
{
    [SerializeField]
    private Projectile projectile;

    [SerializeField]
    private Transform rightHandIKTarget;

    [SerializeField]
    private Transform leftHandIKTarget;

    [SerializeField]
    private Transform rightElbowHint;

    [SerializeField]
    private Transform leftElbowHint;

    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private Vector2 smoothDeltaPosition = Vector2.zero;
    private Vector2 velocity = Vector2.zero;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent.updateRotation = false;
        navMeshAgent.updatePosition = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (projectile.currentState == ProjectileState.WAITING)
        {
            Vector3? mouseGroundPoint = RayCastCameraToGround();
            navMeshAgent.isStopped = false;
            if (Input.GetMouseButtonUp(1))
            {
                
                if (mouseGroundPoint != null)
                {
                    navMeshAgent.destination = (Vector3)mouseGroundPoint;
                }
            }
            
            if (mouseGroundPoint != null)
            {
                Vector3 lookPoint = new Vector3(((Vector3)mouseGroundPoint).x, transform.position.y, ((Vector3)mouseGroundPoint).z);
                transform.LookAt(lookPoint);
            }

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

            // Update animation parameters
            animator.SetBool("ShouldMove", shouldMove);
            animator.SetFloat("velX", velocity.x);
            animator.SetFloat("velY", velocity.y);

            if (Input.GetMouseButtonDown(0))
            {
                projectile.Fire();
            }
        }
        else
        {
            navMeshAgent.destination = transform.position;
            navMeshAgent.isStopped = true;
        }
    }

    void OnAnimatorIK()
    {
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, .75f);
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandIKTarget.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandIKTarget.rotation);
        animator.SetIKHintPosition(AvatarIKHint.RightElbow, rightElbowHint.position);

        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, .75f);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKTarget.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIKTarget.rotation);
        animator.SetIKHintPosition(AvatarIKHint.LeftElbow, leftElbowHint.position);
    }

    void OnAnimatorMove()
    {
        transform.position = navMeshAgent.nextPosition;
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
