using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State {
        Roaming,
        ChasingTarget,
        ShootingTarget,
        ReturningToStart
    }


    [SerializeField] float minRoamRange = 2f, maxRoamRange = 10f, chaseTargetRange = 15f, attackRange = 10f, stopChaseRange = 25f;
    [SerializeField] bool doesRoam = true;
    [SerializeField] private Transform headTransform;
    [SerializeField] private Collider headCollider;
    private State state;
    private Vector3 startingPosition, roamLocation;
    private float lengthWaited, waitAfterStopping;
    private bool isDead = false;


    // References
    private UnityEngine.AI.NavMeshAgent agent;
    private Animator anim;
    private Weapon weapon;



    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();   
        anim = GetComponentInChildren<Animator>();
        weapon = GetComponentInChildren<Weapon>();
        state = State.Roaming;
        
    }

    void Start()
    {
        startingPosition = transform.position;
        roamLocation = GetRandomRoamLocation();
        lengthWaited = 0f;
        waitAfterStopping = Random.Range(2f, 5f);

        SetRigidbodyState(true);
        SetColliderState(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        switch(state)
        {
            default:
            case State.Roaming:
                RoamingState();           
                break;
            case State.ChasingTarget:
                ChasingTargetState();
                break;
            case State.ShootingTarget:
                ShootingTargetState();
                break;
            case State.ReturningToStart:
                ReturningToStartState();
                break;
        }
        
        anim.SetFloat("moveSpeed", agent.velocity.magnitude);
    }

    private Vector3 GetRandomRoamLocation()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;

        UnityEngine.AI.NavMeshHit hit;
        UnityEngine.AI.NavMesh.SamplePosition(startingPosition + randomDirection * Random.Range(minRoamRange, maxRoamRange), out hit, 15f, 1);

        return hit.position;
    }

    private void FindTarget()
    {
        if (Vector3.Distance(transform.position, CharacterControllerNew.instance.GetPosition()) < chaseTargetRange && GetAngleToTarget() < 45)
        {
            // Player is within the target range
            state = State.ChasingTarget;
        }
    }

    private IEnumerator FiringFinished()
    {
        yield return new WaitForSeconds(.5f);
        state = State.ChasingTarget;
    }

    private void RoamingState()
    {
        if (doesRoam)
        {
            agent.SetDestination(roamLocation);
            agent.speed = 3f;
            if (Vector3.Distance(transform.position, roamLocation) < 2f)
            {
                if (lengthWaited >= waitAfterStopping)
                {
                    // Reached the roam location
                    roamLocation = GetRandomRoamLocation();
                    lengthWaited = 0f;
                    waitAfterStopping = Random.Range(2f, 5f);
                }
                lengthWaited += Time.deltaTime;
            }
        }
        FindTarget();
    }

    private void ChasingTargetState()
    {
        agent.SetDestination(CharacterControllerNew.instance.GetPosition());
        agent.speed = 6f;
        float distToPlayer = Vector3.Distance(transform.position, CharacterControllerNew.instance.GetPosition());
        if(distToPlayer < attackRange)
        {
            // Target is within attack range
            state = State.ShootingTarget;
        }
        if (distToPlayer > stopChaseRange)
        {
            // Target is out of range, stop chasing
            state = State.ReturningToStart;
        }
    }

    private void ShootingTargetState()
    {
        agent.SetDestination(transform.position);
                
        weapon.AimAtPlayer();
        headTransform.LookAt(CharacterControllerNew.instance.GetPosition() + new Vector3(0f, Random.Range(0f, .3f), 0f));

        // Rotate towards the Player on the Y axis
        Vector3 lookPos = CharacterControllerNew.instance.GetPosition();
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);

        // Only fire within a 30 degree cone of player position
        if (GetAngleToTarget() < 30f)
        {
            RaycastHit hit;
            if (Physics.Raycast(headTransform.position, headTransform.forward, out hit, attackRange + 5f) && hit.collider.tag == "Player")
            {
                weapon.FireWeapon();
                StartCoroutine(FiringFinished());
            }
        }
    }

    private void ReturningToStartState()
    {
        agent.SetDestination(startingPosition);
        agent.speed = 3f;

        if (Vector3.Distance(transform.position, startingPosition) < 5f)
        {
            // Reached start position
            state = State.Roaming;
            lengthWaited = 0f;
            waitAfterStopping = Random.Range(2f, 5f);
        }
    }

    public void WasShot()
    {
        if (state == State.Roaming || state == State.ReturningToStart)
        {
            state = State.ChasingTarget;
        }
    }

    private float GetAngleToTarget()
    {
        // Check the angle to the player
        Vector3 targetDirection = CharacterControllerNew.instance.GetPosition() - transform.position;
        float angle = Vector3.SignedAngle(targetDirection, transform.forward, Vector3.up);
        return Mathf.Abs(angle);
    }

    void OnDrawGizmos()
    {
        if (agent != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(agent.destination, 1);
        }
        if (doesRoam)
        {
            Gizmos.DrawWireSphere(transform.position, maxRoamRange);
        }
    }

    private void SetRigidbodyState(bool state)
    {
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rb in rbs)
        {
            rb.isKinematic = state;
        }
    }

    private void SetColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach(Collider collider in colliders)
        {
            collider.enabled = state;
        }

        GetComponent<Collider>().enabled = !state;
        headCollider.enabled = !state;
    }

    public void EnemyDied()
    {
        isDead = true;
        agent.isStopped = true;

        anim.enabled = false;
        SetRigidbodyState(false);
        SetColliderState(true);

        Destroy(gameObject, 3f);
    }

}
