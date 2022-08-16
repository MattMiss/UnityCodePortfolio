using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float distanceToChase = 10f, distanceToLose = 15f, distanceToStop = 2f, keepChasingTime;
    private float chaseCounter;
    private bool chasing;
    private Vector3 targetPoint, startPoint;

    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform muzzlePoint;
    [SerializeField]
    private float fireRate, waitBetweenShots, timeToShoot = 1f;
    private float fireCount, shotWaitCount, shootTimeCount;

    // References
    private NavMeshAgent agent;
    private Animator anim;

    private bool wasShot;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        startPoint = transform.position;

        shootTimeCount = timeToShoot;
        shotWaitCount = waitBetweenShots;
    }

    void Update()
    {
        targetPoint = PlayerControllerOld.instance.transform.position;
        targetPoint.y = transform.position.y;

        if (!chasing)
        {
            if (Vector3.Distance(transform.position, targetPoint) < distanceToChase)
            {
                chasing = true;

                shootTimeCount = timeToShoot;
                shotWaitCount = waitBetweenShots;
            }

            if (chaseCounter > 0)
            {
                chaseCounter -= Time.deltaTime;
                if (chaseCounter <= 0)
                {
                    agent.destination = startPoint;
                }
            }
            anim.SetBool("isMoving", agent.remainingDistance > .25f);
        }
        else
        {
            if (Vector3.Distance(transform.position, targetPoint) > distanceToStop)
            {
                agent.destination = targetPoint;
            }
            else
            {
                agent.destination = transform.position;
            }

            if (Vector3.Distance(transform.position, targetPoint) > distanceToLose)
            {
                if (!wasShot)
                {
                    chasing = false;
                    chaseCounter = keepChasingTime;
                }        
            }
            else
            {
                wasShot = false;
            }

            if (shotWaitCount > 0)
            {
                shotWaitCount -= Time.deltaTime;  
                if (shotWaitCount <= 0)
                {
                    shootTimeCount = timeToShoot;
                }
                anim.SetBool("isMoving", true);     
            }
            else
            {
                if (PlayerControllerOld.instance.gameObject.activeInHierarchy)
                {
                    shootTimeCount -= Time.deltaTime;
                    if (shootTimeCount > 0)
                    {
                        fireCount -= Time.deltaTime;

                        if (fireCount <= 0)
                        {
                            fireCount = fireRate;

                            // Raise the muzzlepoint slightly
                            muzzlePoint.LookAt(PlayerControllerOld.instance.transform.position + new Vector3(0f, Random.Range(0f, .3f), 0f));

                            // Check the angle to the player
                            Vector3 targetDirection = PlayerControllerOld.instance.transform.position - transform.position;
                            float angle = Vector3.SignedAngle(targetDirection, transform.forward, Vector3.up);
                            // Only fire within a 30 degree cone
                            if (Mathf.Abs(angle) < 30f)
                            {
                                Instantiate(bullet, muzzlePoint.position, muzzlePoint.rotation);

                                anim.SetTrigger("fireShot");
                            }
                            else
                            {
                                shotWaitCount = waitBetweenShots;
                            }

                        }

                        // Stop moving while shooting
                        agent.destination = transform.position;
                    }
                    else
                    {
                        shotWaitCount = waitBetweenShots;
                    }

                    anim.SetBool("isMoving", false);
                }

            }


        }

    }

    public void GetShot()
    {
        wasShot = true;
        chasing = true;
    }
}
